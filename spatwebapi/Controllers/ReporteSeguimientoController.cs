using System.Collections.Generic;
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
    [AllowAnonymous]
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
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            var lista = _reporteSeguimientoService.GetAll();
            var mapped = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var reporte = await _reporteSeguimientoService.GetById(id);
            if (reporte == null)
                return NotFound(null);
            var mapped = _mapper.Map<ReporteSeguimientoForReturn>(reporte);
            return Ok(mapped);
        }
        [Authorize(Roles = "Voluntario")]
        [HttpGet("{id}/GetReportesForVoluntario")]
        public IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id)
        {
            var lista = _reporteSeguimientoService.GetReportesForVoluntario(id);
            return lista;
        }
        //[HttpPost("CrearReporte")]
        //public async Task<IActionResult> GuardarReporte(ReporteSeguimientoForCreate reporteDto)
        //{
        //    var seguimiento = await _reporteSeguimientoService.GuardarReporte(reporteDto);
        //    if (seguimiento!=null)
        //    {
        //        var mapped = _mapper.Map<ReporteSeguimientoForReturn>(seguimiento);
        //        return Ok(mapped);
        //    }
        //    return BadRequest(new { mensaje = "No se encuentra el Seguimiento." });
        //}
        [HttpPost("CreateReporteSeguimiento/{id}")]
        public async Task<IActionResult> CreateReporteSeguimiento(int id)
        {
            if (await _reporteSeguimientoService.CreateReporteSeguimiento(id))
            {
                var seg = _reporteSeguimientoService.GetReportesForAdmin(id);
                return Ok(seg);
            }

            return BadRequest(new { mensaje = "Hubo problemas al agregar el reporte." });
        }
        [HttpPut("SendReporte")]
        public async Task<IActionResult> SendReporte([FromForm] ReporteSeguimientoForUpdate reporte, IFormFile Foto)
        {
            var modelo = await _reporteSeguimientoService.GetByIdNotracking(reporte.Id);
            if (modelo == null)
                return NotFound(new { mensaje = "Reporte no encontrado, Id incorrecto." });
            if (modelo.Estado.Equals("Enviado"))
                return BadRequest(new { mensaje = "El reporte ya fue enviado anteriormente." });

            var resulMascota = await _reporteSeguimientoService.SendReporte(reporte);
            if (resulMascota)
            {
                var resulFoto = await _fotoService.AgregarFotoReporte(reporte.Id, Foto);
                if (resulFoto)
                {
                    var seguimiento = await _seguimientoService.GetById(reporte.SeguimientoId);
                    var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                    return Ok(mapped);
                }
            }
            return BadRequest(new { mensaje = "Hubo problemas al generar el Reporte." });
        }
        [HttpPut("UpdateFecha")]
        public async Task<IActionResult> UpdateFecha(ReporteSeguimientoForUpdateAdmin reporte)
        {
            var modelo = await _reporteSeguimientoService.GetById(reporte.Id);
            if (modelo.Estado.Equals("Enviado"))
                return BadRequest(new { mensaje = "El Reporte ya fue enviado." });
            var valor = await _reporteSeguimientoService.VerifyDate(reporte);
            if (valor == 1)
            {
                var resul = await _reporteSeguimientoService.UpdateFecha(reporte);
                if (resul)
                {
                    var seg = _reporteSeguimientoService.GetReportesForAdmin(modelo.SeguimientoId);
                    return Ok(seg);
                }
                return BadRequest(new { mensaje = "Hubo problemas al actualizar el reporte" });
            }
            else if (valor == 2)
                return BadRequest(new { mensaje = "La fecha no debe ser la misma que la de otro reporte creado." });
            else
                return BadRequest(new { mensaje = "La fecha debe estar en el rango establecido en el seguimiento." });
        }
        [HttpDelete("{idseguimiento}/DeleteReporte/{id}")]
        public async Task<IActionResult> DeleteReporte(int idseguimiento, int id)
        {
            if (await _reporteSeguimientoService.DeleteReporte(id))
            {
                var seg = _reporteSeguimientoService.GetReportesForAdmin(idseguimiento);
                return Ok(seg);
            }
            return BadRequest(new { mensaje = "Hubo un problema al eliminar el Reporte." });
        }
    }
}