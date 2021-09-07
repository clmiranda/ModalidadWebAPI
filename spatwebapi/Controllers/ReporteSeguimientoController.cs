﻿using System.Collections.Generic;
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
                seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderBy(x => x.Fecha.ToShortDateString()).ToList();
                var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "Hubo problemas al agregar el reporte." });
        }
        [HttpPut("SendReporte")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> SendReporte([FromForm] ReporteSeguimientoForUpdate reporteDto, IFormFile Foto)
        {
            var reporte = await _reporteSeguimientoService.GetByIdNotracking(reporteDto.Id);
            if (reporte == null)
                return NotFound(new { mensaje = "Reporte no encontrado, Id incorrecto." });
            if (reporte.Estado.Equals("Enviado"))
                return BadRequest(new { mensaje = "El reporte ya fue enviado anteriormente." });

            var resultadoMascota = await _reporteSeguimientoService.SendReporte(reporteDto);
            if (resultadoMascota)
            {
                var resultadoFoto = await _fotoService.AgregarFotoReporte(reporteDto.Id, Foto);
                if (resultadoFoto)
                {
                    var seguimiento = await _seguimientoService.GetById(reporteDto.SeguimientoId);
                    var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                    return Ok(mapped);
                }
            }
            return BadRequest(new { mensaje = "Hubo problemas al generar el Reporte." });
        }
        [HttpPut("UpdateFecha")]
        public async Task<IActionResult> UpdateFecha(ReporteSeguimientoForUpdateAdmin reporteDto)
        {
            var reporte = await _reporteSeguimientoService.GetById(reporteDto.Id);
            if (reporte.Estado.Equals("Enviado"))
                return BadRequest(new { mensaje = "El Reporte ya fue enviado." });
            var verificar = await _reporteSeguimientoService.VerifyDate(reporteDto);
            if (verificar == 1)
            {
                var resultado = await _reporteSeguimientoService.UpdateFecha(reporteDto);
                if (resultado)
                {
                    var seguimiento = await _reporteSeguimientoService.GetReportesForAdmin(reporte.SeguimientoId);
                    seguimiento.ReporteSeguimientos=seguimiento.ReporteSeguimientos.OrderByDescending(x => x.Fecha).ToList();
                    var mappeado = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                    return Ok(mappeado);
                }
                return BadRequest(new { mensaje = "Hubo problemas al actualizar el reporte" });
            }
            else if (verificar == 2)
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