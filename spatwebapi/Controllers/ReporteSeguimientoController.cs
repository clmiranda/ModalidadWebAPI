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
        [HttpGet("{id}/ListReporteSeguimiento")]
        public IEnumerable<ReporteSeguimientoForReturn> ListReporteSeguimiento(int id)
        {
            var lista = _reporteSeguimientoService.GetByCondition(id);
            return lista;
        }
        [HttpPost("CreateReporteSeguimiento")]
        public async Task<IActionResult> CreateReporteSeguimiento(ReporteSeguimientoForCreate reporteDto)
        {
            if (await _reporteSeguimientoService.CreateReporteSeguimiento(reporteDto))
                return Ok("El Reporte fue agregado exitosamente.");

            return BadRequest("Hubo problemas al agregar el Reporte");
        }
        [HttpPut("UpdateReporteSeguimiento")]
        public async Task<IActionResult> UpdateReporteSeguimiento(ReporteSeguimientoForUpdate reporte)
        {
            var modelo = await _reporteSeguimientoService.GetById(reporte.Id);
            if (modelo.Estado.Equals("Enviado"))
                return BadRequest("El reporte ya fue enviado anteriormente.");

            if (await _reporteSeguimientoService.UpdateReporteSeguimiento(reporte))
                return Ok("El Reporte fue enviado exitosamente.");

            return BadRequest("Hubo problemas al enviar el Reporte");
        }
    }
}