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
        private readonly IMapper _mapper;
        public ReporteSeguimientoController(IReporteSeguimientoService reporteSeguimientoService, IMapper mapper)
        {
            _reporteSeguimientoService = reporteSeguimientoService;
            _mapper = mapper;
        }
        [HttpGet("{id}/GetById")]
        public async Task<ReporteSeguimientoForReturn> GetById(int id)
        {
            var reporte = _mapper.Map<ReporteSeguimientoForReturn>(await _reporteSeguimientoService.GetById(id));
            return reporte;
        }
        [Authorize(Roles ="Administrador")]
        [HttpGet("{id}/GetReportesForAdmin")]
        public IEnumerable<ReporteSeguimientoForReturn> GetReportesForAdmin(int id)
        {
            var lista = _reporteSeguimientoService.GetReportesForAdmin(id);
            return lista;
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
        [HttpPost("CreateReporteSeguimiento")]
        public async Task<IActionResult> CreateReporteSeguimiento(ReporteSeguimiento reporte)
        {
            //if (await _reporteSeguimientoService.VerifyMaximoReportes(reporteDto.SeguimientoId))
            //{
            if (await _reporteSeguimientoService.CreateReporteSeguimiento(reporte)) {
                var lista = _reporteSeguimientoService.GetReportesForAdmin(reporte.SeguimientoId);
                return Ok(lista);
            }

                return BadRequest("Hubo problemas al agregar el Reporte");
            //}
            //return BadRequest("No se pueden asignar más de 10 Reportes por Seguimiento.");
        }
        [HttpPut("UpdateReporteSeguimientoVoluntario")]
        public async Task<IActionResult> UpdateReporteSeguimientoVoluntario(ReporteSeguimientoForUpdate reporte)
        {
            var modelo = await _reporteSeguimientoService.GetById(reporte.Id);
            if (modelo.Estado.Equals("Enviado"))
                return BadRequest("El reporte ya fue enviado anteriormente.");

            if (await _reporteSeguimientoService.UpdateReporteSeguimientoVoluntario(reporte))
                return Ok("El Reporte fue enviado exitosamente.");

            return BadRequest("Hubo problemas al enviar el Reporte");
        }
        [HttpPut("SaveReporteSeguimientoAdmin")]
        public async Task<IActionResult> SaveReporteSeguimientoAdmin(ReporteSeguimientoForUpdateAdmin reporte)
        {
            var modelo = await _reporteSeguimientoService.GetById(reporte.Id);
            if (modelo.Estado.Equals("Enviado"))
                return BadRequest("El Reporte ya fue enviado anteriormente.");
            if (await _reporteSeguimientoService.VerifyDate(reporte))
            {
                if (await _reporteSeguimientoService.UpdateReporteSeguimientoAdmin(reporte))
                    return Ok("El Reporte fue actualizado exitosamente.");
            }
            else
                return BadRequest("La fecha no debe ser misma que la de otro reporte y debe estar en el " +
                    "rango establecido.");

            return BadRequest("Hubo problemas al actualizar el Reporte");
        }
        [HttpDelete("{idseguimiento}/DeleteReporte/{id}")]
        public async Task<IActionResult> DeleteReporte(int idseguimiento, int id)
        {
            if (await _reporteSeguimientoService.VerifyMinimoReportes(idseguimiento))
            {
                if (await _reporteSeguimientoService.DeleteReporte(id))
                    return Ok("Reporte eliminado correctamente.");

                return BadRequest("Hubo un problema al eliminar el Reporte.");
            }
            return BadRequest("El mínimo de Reportes por Seguimiento debe ser de 3.");
        }
    }
}