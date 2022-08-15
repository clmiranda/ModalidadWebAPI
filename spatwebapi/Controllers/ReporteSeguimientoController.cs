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
        [HttpGet("GetAllReporteSeguimientosForReport")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> GetAllReporteSeguimientosForReport()
        {
            var lista = await _reporteSeguimientoService.GetAllReporteSeguimientosForReport();
            var mapped = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(lista);
            return Ok(mapped);
        }
        [HttpPut("UpdateRangoFechasSeguimiento")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> UpdateRangoFechasSeguimiento([FromBody] RangoFechaSeguimientoDto rangoFechaSeguimientoDto)
        {
            var seguimiento = await _seguimientoService.GetById(rangoFechaSeguimientoDto.Id);
            if (seguimiento != null)
            {
                var resultado = await _reporteSeguimientoService.UpdateRangoFechasSeguimiento(rangoFechaSeguimientoDto);
                if (resultado != null)
                {
                    var mapped = _mapper.Map<SeguimientoForReturnDto>(resultado);
                    mapped.ReporteSeguimientos = mapped.ReporteSeguimientos.OrderBy(x => x.FechaReporte).ToList().OrderBy(x => x.Estado).ToList();
                    return Ok(mapped);
                }
                return BadRequest(new { mensaje = "Problemas al actualizar los datos." });
            }
            return BadRequest(new { mensaje = "No existe el Seguimiento." });
        }
        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> GetById(int id)
        {
            var reporteSeguimiento = await _reporteSeguimientoService.GetById(id);
            if (reporteSeguimiento == null)
                return NotFound(null);
            var mapped = _mapper.Map<ReporteSeguimientoForReturn>(reporteSeguimiento);
            return Ok(mapped);
        }
        [HttpPost("CreateReporteSeguimiento/{id}")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> CreateReporteSeguimiento(int id)
        {
            if (await _reporteSeguimientoService.CreateReporteSeguimiento(id))
            {
                var seguimiento = await _reporteSeguimientoService.GetReportesForAdmin(id);
                seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderBy(x => x.FechaReporte).ToList().OrderBy(x => x.Estado).ToList();
                var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "Hubo problemas al agregar el reporte." });
        }
        [HttpPut("SendReporte")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> SendReporte([FromForm] ReporteSeguimientoForUpdate reporteSeguimientoDto, IFormFile Foto)
        {
            var reporteSeguimiento = await _reporteSeguimientoService.GetByIdNotracking(reporteSeguimientoDto.Id);
            if (reporteSeguimiento == null)
                return NotFound(new { mensaje = "Reporte no encontrado, Id incorrecto." });
            if (reporteSeguimiento.Estado.Equals("Enviado"))
                return BadRequest(new { mensaje = "El reporte ya fue enviado anteriormente." });

            var resultado = await _reporteSeguimientoService.SendReporte(reporteSeguimientoDto, Foto);
            if (resultado)
            {
                var seguimiento = await _seguimientoService.GetById(reporteSeguimientoDto.SeguimientoId);
                seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderBy(x => x.FechaReporte.Date).ToList().OrderBy(x => x.Estado).ToList().FindAll(x => !x.Estado.Equals("Activo")).ToList();
                var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "Hubo problemas al generar el reporte." });
        }
        [HttpPut("UpdateFechaReporte")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> UpdateFechaReporte(ReporteSeguimientoForUpdateAdmin reporteSeguimientoDto)
        {
            var reporteSeguimiento = await _reporteSeguimientoService.GetById(reporteSeguimientoDto.Id);
            if (reporteSeguimiento.Estado.Equals("Enviado"))
                return BadRequest(new { mensaje = "El Reporte ya fue enviado." });
            var verifyDate = await _reporteSeguimientoService.VerifyDate(reporteSeguimientoDto);
            if (verifyDate == 1)
            {
                var resultado = await _reporteSeguimientoService.UpdateFechaReporte(reporteSeguimientoDto);
                if (resultado)
                {
                    var seguimiento = await _reporteSeguimientoService.GetReportesForAdmin(reporteSeguimiento.SeguimientoId);
                    seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderBy(x => x.FechaReporte).ToList().OrderBy(x => x.Estado).ToList();
                    var mappeado = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                    return Ok(mappeado);
                }
                return BadRequest(new { mensaje = "Hubo problemas al actualizar el reporte" });
            }
            else if (verifyDate == 2)
                return BadRequest(new { mensaje = "La fecha no debe ser la misma que la de otro reporte creado." });
            else if (verifyDate == 3)
                return BadRequest(new { mensaje = "La fecha debe estar en el rango establecido en el seguimiento." });
            else
                return BadRequest(new { mensaje = "La fecha no puede ser menor a la fecha actual." });
        }
        [HttpDelete("{idSeguimiento}/DeleteReporte/{idReporte}")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> DeleteReporte(int idSeguimiento, int idReporte)
        {
            if (await _reporteSeguimientoService.DeleteReporte(idReporte))
            {
                var seguimiento = await _reporteSeguimientoService.GetReportesForAdmin(idSeguimiento);
                seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderBy(x => x.FechaReporte).ToList().OrderBy(x => x.Estado).ToList();
                var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "Hubo un problema al eliminar el Reporte." });
        }
    }
}