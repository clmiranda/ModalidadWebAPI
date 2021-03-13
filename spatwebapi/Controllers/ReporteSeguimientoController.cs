using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Dtos.Usuario;
using webapi.business.Services.Interf;
using webapi.core.Models;

namespace spatwebapi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteSeguimientoController : Controller
    {
        private IReporteSeguimientoService _reporteSeguimientoService;
        private IFotoService _fotoService;
        private readonly IMapper _mapper;
        public ReporteSeguimientoController(IReporteSeguimientoService reporteSeguimientoService, IFotoService fotoService, IMapper mapper)
        {
            _reporteSeguimientoService = reporteSeguimientoService;
            _fotoService = fotoService;
            _mapper = mapper;
        }
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var reporte = _mapper.Map<ReporteSeguimientoForReturn>(await _reporteSeguimientoService.GetById(id));
            if (reporte == null)
                return NotFound(null);
            return Ok(reporte);
        }
        [Authorize(Roles ="Administrador")]
        [HttpGet("{id}/GetReportesForAdmin")]
        public SeguimientoForReturnDto GetReportesForAdmin(int id)
        {
            var seg = _reporteSeguimientoService.GetReportesForAdmin(id);
            return seg;
        }
        [Authorize(Roles = "Voluntario")]
        [HttpGet("{id}/GetReportesForVoluntario")]
        public IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id)
        {
            var lista = _reporteSeguimientoService.GetReportesForVoluntario(id);
            return lista;
        }
        [HttpPost("CrearReporte")]
        public async Task<IActionResult> CrearReporte(ReporteSeguimientoForCreate reporteDto)
        {
            var seguimiento = await _reporteSeguimientoService.CreateReporte(reporteDto);
            if (seguimiento!=null)
            {
                var mapped = _mapper.Map<ReporteSeguimientoForReturn>(seguimiento);
                return Ok(mapped);
            }
            return BadRequest("No se encuentra el Seguimiento.");
        }
        [HttpPost("CreateReporteSeguimiento/{id}")]
        public async Task<IActionResult> CreateReporteSeguimiento(int id)
        {
            //if (await _reporteSeguimientoService.VerifyMaximoReportes(reporteDto.SeguimientoId))
            //{
            if (await _reporteSeguimientoService.CreateReporteSeguimiento(id)) {
                var seg = _reporteSeguimientoService.GetReportesForAdmin(id);
                return Ok(seg);
            }

                return BadRequest(new { mensaje = "Hubo problemas al agregar el reporte." });
            //}
            //return BadRequest("No se pueden asignar más de 10 Reportes por Seguimiento.");
        }
        [HttpPut("UpdateReporteSeguimientoVoluntario")]
        public async Task<IActionResult> UpdateReporteSeguimientoVoluntario([FromForm]ReporteSeguimientoForUpdate reporte)
        {
            var modelo = await _reporteSeguimientoService.GetById(reporte.Id);
            if (modelo == null)
                return BadRequest(new { mensaje = "Reporte no encontrado, Id incorrecto." });
            if (modelo.Estado.Equals("Enviado"))
                return BadRequest(new { mensaje = "El reporte ya fue enviado anteriormente." });

            var r = await _reporteSeguimientoService.UpdateReporteSeguimientoVoluntario(reporte);
            if (r != null) {
                var resul = await _fotoService.AgregarFotoReporte(reporte.SeguimientoId, reporte.Foto);
                if (resul)
                    return Ok(r);
            }
            return BadRequest(new { mensaje = "Hubo problemas al generar el Reporte." });
        }
        [HttpPut("SaveReporteSeguimientoAdmin")]
        public async Task<IActionResult> SaveReporteSeguimientoAdmin(ReporteSeguimientoForUpdateAdmin reporte)
        {
            var modelo = await _reporteSeguimientoService.GetById(reporte.Id);
            if (modelo.Estado.Equals("Enviado"))
                return BadRequest("El Reporte ya fue enviado.");
            var valor = await _reporteSeguimientoService.VerifyDate(reporte);
            //switch (valor)
            //{
            //    case 1:
            //        if (await _reporteSeguimientoService.UpdateReporteSeguimientoAdmin(reporte))
            //        {
            //            var seg = _reporteSeguimientoService.GetReportesForAdmin(modelo.SeguimientoId);
            //            return Ok(seg);
            //        }
            //        else
            //            break;
            //    case 2:
            //        return BadRequest("La fecha no debe ser la misma que la de otro reporte.");
            //    case 3:
            //        return BadRequest("La fecha debe estar en el rango establecido en el seguimiento.");
            //}
            //return BadRequest("Ocurrio un error al actualizar la fecha.");
            if (valor != 3)
            {
                if (valor != 2)
                {
                    if (await _reporteSeguimientoService.UpdateReporteSeguimientoAdmin(reporte))
                    {
                        var seg = _reporteSeguimientoService.GetReportesForAdmin(modelo.SeguimientoId);
                        return Ok(seg);
                    }
                    return BadRequest(new { mensaje = "Hubo problemas al actualizar el Reporte" });
                }
                return BadRequest(new { mensaje = "La fecha no debe ser la misma que la de otro reporte." });
            }
                return BadRequest(new { mensaje = "La fecha debe estar en el rango establecido en el seguimiento." });
        }
        [HttpDelete("{idseguimiento}/DeleteReporte/{id}")]
        public async Task<IActionResult> DeleteReporte(int idseguimiento, int id)
        {
            //if (await _reporteSeguimientoService.VerifyMinimoReportes(idseguimiento))
            //{
                if (await _reporteSeguimientoService.DeleteReporte(id))
                {
                    var seg = _reporteSeguimientoService.GetReportesForAdmin(idseguimiento);
                    return Ok(seg);
                }
                return BadRequest(new { mensaje = "Hubo un problema al eliminar el Reporte." });
            //}
            //return BadRequest("El mínimo de Reportes por Seguimiento debe ser de 3.");
        }
    }
}