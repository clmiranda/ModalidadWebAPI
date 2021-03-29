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
using webapi.business.Services.Imp;
using webapi.business.Services.Interf;
using webapi.core.Models;

namespace spatwebapi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteSeguimientoController : Controller
    {
        private ISeguimientoService _seguimientoService;
        private IReporteSeguimientoService _reporteSeguimientoService;
        private IFotoService _fotoService;
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
            var reporte = _mapper.Map<ReporteSeguimientoForReturn>(await _reporteSeguimientoService.GetById(id));
            if (reporte == null)
                return NotFound(null);
            return Ok(reporte);
        }
        //[Authorize(Roles ="Administrador")]
        //[HttpGet("{id}/GetReportesForAdmin")]
        //public SeguimientoForReturnDto GetReportesForAdmin(int id)
        //{
        //    var seg = _reporteSeguimientoService.GetReportesForAdmin(id);
        //    return seg;
        //}
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
        public async Task<IActionResult> UpdateReporteSeguimientoVoluntario([FromForm]ReporteSeguimientoForUpdate reporte, IFormFile Foto)
        {
            var modelo = await _reporteSeguimientoService.GetByIdNotracking(reporte.Id);
            if (modelo == null)
                return BadRequest(new { mensaje = "Reporte no encontrado, Id incorrecto." });
            if (modelo.Estado.Equals("Enviado"))
                return BadRequest(new { mensaje = "El reporte ya fue enviado anteriormente." });

            var r = await _reporteSeguimientoService.UpdateReporteSeguimientoVoluntario(reporte);
            if (r) {
                var resul = await _fotoService.AgregarFotoReporte(reporte.Id, Foto);
                if (resul)
                {
                    var seguimiento = await _seguimientoService.GetById(reporte.SeguimientoId);
                    var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                    return Ok(mapped);
                }
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
            if (valor==1)
            {
                var resul = await _reporteSeguimientoService.UpdateReporteSeguimientoAdmin(reporte);
                if (resul)
                {
                    var seg = _reporteSeguimientoService.GetReportesForAdmin(modelo.SeguimientoId);
                    return Ok(seg);
                }
                return BadRequest(new { mensaje = "Hubo problemas al actualizar el reporte" });
            }
            else if (valor == 2)
            {
                return BadRequest(new { mensaje = "La fecha no debe ser la misma que la de otro reporte creado." });
            }
            else
            {
                return BadRequest(new { mensaje = "La fecha debe estar en el rango establecido en el seguimiento." });
            }
            //if (valor != 3)
            //{
            //    if (valor != 2)
            //    {
            //        if (await _reporteSeguimientoService.UpdateReporteSeguimientoAdmin(reporte))
            //        {
            //            var seg = _reporteSeguimientoService.GetReportesForAdmin(modelo.SeguimientoId);
            //            return Ok(seg);
            //        }
            //        return BadRequest(new { mensaje = "Hubo problemas al actualizar el reporte" });
            //    }
            //    return BadRequest(new { mensaje = "La fecha no debe ser la misma que la de otro reporte creado." });
            //}
            //    return BadRequest(new { mensaje = "La fecha debe estar en el rango establecido en el seguimiento." });
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