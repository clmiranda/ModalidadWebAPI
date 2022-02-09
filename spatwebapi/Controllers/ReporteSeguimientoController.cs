using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Services.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdministrador, Administrador")]
    public class ReporteSeguimientoController : Controller
    {
        private readonly ISeguimientoService _seguimientoService;
        private readonly IReporteSeguimientoService _reporteSeguimientoService;
        private readonly IFotoService _fotoService;
        private readonly IMapper _mapper;
        public ReporteSeguimientoController(ISeguimientoService seguimientoService, IReporteSeguimientoService reporteSeguimientoService, IFotoService fotoService, IMapper mapper)
        {
            _seguimientoService = seguimientoService;
            _reporteSeguimientoService = reporteSeguimientoService;
            _fotoService = fotoService;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            var lista = _reporteSeguimientoService.GetAll();
            var mapped = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(lista);
            return Ok(mapped);
        }
        [HttpPut("UpdateRangoFechasSeguimiento")]
        public async Task<IActionResult> UpdateRangoFechasSeguimiento([FromBody] RangoFechaSeguimientoDto rangoFechaSeguimientoDto) {
            var seguimiento = await _seguimientoService.GetById(rangoFechaSeguimientoDto.Id);
            if (seguimiento != null) {
                var resultado = await _reporteSeguimientoService.UpdateRangoFechasSeguimiento(rangoFechaSeguimientoDto);
                if (resultado != null) {
                    var mapped = _mapper.Map<SeguimientoForReturnDto>(resultado);
                    mapped.ReporteSeguimientos = mapped.ReporteSeguimientos.OrderByDescending(x => x.FechaReporte).ToList().OrderByDescending(x => x.Id).ToList();
                    return Ok(mapped);
                }
                return BadRequest(new { mensaje = "Problemas al actualizar los datos." });
            }
            return BadRequest(new { mensaje = "No existe el Seguimiento." });
        }
        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<ActionResult> GetById(int id)
        {
            var reporte = await _reporteSeguimientoService.GetById(id);
            if (reporte == null)
                return NotFound(null);
            var mapped = _mapper.Map<ReporteSeguimientoForReturn>(reporte);
            return Ok(mapped);
        }
        [HttpGet("{id}/GetReportesForVoluntario")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id)
        {
            var lista = _reporteSeguimientoService.GetReportesForVoluntario(id);
            return lista;
        }
        [HttpPost("CreateReporteSeguimiento/{id}")]
        public async Task<IActionResult> CreateReporteSeguimiento(int id)
        {
            if (await _reporteSeguimientoService.CreateReporteSeguimiento(id))
            {
                var seguimiento = await _reporteSeguimientoService.GetReportesForAdmin(id);
                seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderByDescending(x => x.FechaReporte).ToList().OrderBy(x => x.Estado).ToList();
                var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "Hubo problemas al agregar el reporte." });
        }
        [HttpPut("SendReporte")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> SendReporte([FromForm] ReporteSeguimientoForUpdate reporteSeguimientoDto, IFormFile Foto)
        {
            var reporte = await _reporteSeguimientoService.GetByIdNotracking(reporteSeguimientoDto.Id);
            if (reporte == null)
                return NotFound(new { mensaje = "Reporte no encontrado, Id incorrecto." });
            if (reporte.Estado.Equals("Enviado"))
                return BadRequest(new { mensaje = "El reporte ya fue enviado anteriormente." });

            var resultadoMascota = await _reporteSeguimientoService.SendReporte(reporteSeguimientoDto);
            if (resultadoMascota)
            {
                var resultadoFoto = await _fotoService.AgregarFotoReporte(reporteSeguimientoDto.Id, Foto);
                if (resultadoFoto)
                {
                    var seguimiento = await _seguimientoService.GetById(reporteSeguimientoDto.SeguimientoId);
                    seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderByDescending(x => x.FechaReporte.Date.Equals(DateTime.Now.Date)).ToList().OrderBy(x => x.Estado).ToList().FindAll(x => !x.Estado.Equals("Activo")).ToList();
                    var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                    return Ok(mapped);
                }
            }
            return BadRequest(new { mensaje = "Hubo problemas al generar el Reporte." });
        }
        [HttpPut("UpdateFechaReporte")]
        public async Task<IActionResult> UpdateFechaReporte(ReporteSeguimientoForUpdateAdmin reporteSeguimientoDto)
        {
            var reporte = await _reporteSeguimientoService.GetById(reporteSeguimientoDto.Id);
            if (reporte.Estado.Equals("Enviado"))
                return BadRequest(new { mensaje = "El Reporte ya fue enviado." });
            var verificar = await _reporteSeguimientoService.VerifyDate(reporteSeguimientoDto);
            if (verificar == 1)
            {
                var resultado = await _reporteSeguimientoService.UpdateFechaReporte(reporteSeguimientoDto);
                if (resultado)
                {
                    var seguimiento = await _reporteSeguimientoService.GetReportesForAdmin(reporte.SeguimientoId);
                    seguimiento.ReporteSeguimientos=seguimiento.ReporteSeguimientos.OrderByDescending(x => x.FechaReporte).ToList().OrderBy(x => x.Estado).ToList();
                    var mappeado = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                    return Ok(mappeado);
                }
                return BadRequest(new { mensaje = "Hubo problemas al actualizar el reporte" });
            }
            else if (verificar == 2)
                return BadRequest(new { mensaje = "La fecha no debe ser la misma que la de otro reporte creado." });
            else if (verificar == 3)
                return BadRequest(new { mensaje = "La fecha debe estar en el rango establecido en el seguimiento." });
            else
                return BadRequest(new { mensaje = "La fecha no puede ser menor a la fecha actual." });
        }
        [HttpDelete("{idSeguimiento}/DeleteReporte/{idReporte}")]
        public async Task<IActionResult> DeleteReporte(int idSeguimiento, int idReporte)
        {
            if (await _reporteSeguimientoService.DeleteReporte(idReporte))
            {
                var seguimiento = await _reporteSeguimientoService.GetReportesForAdmin(idSeguimiento);
                seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderByDescending(x => x.FechaReporte).ToList().OrderBy(x => x.Estado).ToList();
                var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "Hubo un problema al eliminar el Reporte." });
        }
    }
}