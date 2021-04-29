using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Dtos.ReporteTratamientos;
using webapi.business.Services.Imp;
using webapi.business.Services.Interf;
using webapi.core.Models;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteTratamientoController : Controller
    {
        private readonly IReporteTratamientoService _reporteTratamientoService;
        private readonly IMapper _mapper;
        public ReporteTratamientoController(IReporteTratamientoService reporteTratamientoService,
            IMapper mapper)
        {
            _reporteTratamientoService = reporteTratamientoService;
            _mapper = mapper;
        }
        [HttpGet("GetAll/{id}")]
        public ActionResult GetAll(int id)
        {
            var lista = _reporteTratamientoService.GetAll(id);
            var mapped = _mapper.Map<List<ReporteTratamientoForReturnDto>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetReporteTratamiento/{id}")]
        public async Task<IActionResult> GetReporteTratamiento(int id)
        {
            var reporteTratamiento = await _reporteTratamientoService.GetReporteTratamiento(id);
            if (reporteTratamiento == null) return NotFound(null);
            var mapped = _mapper.Map<ReporteTratamientoForReturnDto>(reporteTratamiento);
            return Ok(mapped);
        }
        [HttpPost("CreateReporteTratamiento")]
        public async Task<IActionResult> CreateReporteTratamiento([FromBody]ReporteTratamientoForCreateDto reporteTratamientoDto)
        {
            var resultado = await _reporteTratamientoService.CreateReporteTratamiento(reporteTratamientoDto);
            if (resultado)
                return Ok();
            return BadRequest(new { mensaje = "Hubo problemas al crear el reporte." });
        }
        [HttpPut("UpdateReporteTratamiento")]
        public async Task<IActionResult> UpdateReporteTratamiento(ReporteTratamientoForUpdateDto reporteTratamientoDto)
        {
            var resultado = await _reporteTratamientoService.UpdateReporteTratamiento(reporteTratamientoDto);
            if (resultado)
                return Ok();
            return BadRequest(new { mensaje = "Hubo problemas al modificar el reporte." });
        }
        [HttpDelete("DeleteReporteTratamiento/{id}")]
        public async Task<IActionResult> DeleteReporteTratamiento(int id)
        {
            var reporteTratamiento = await _reporteTratamientoService.GetReporteTratamiento(id);
            if (reporteTratamiento == null) return NotFound(null);
            if (await _reporteTratamientoService.DeleteReporteTratamiento(reporteTratamiento))
                return Ok();
            else
                return BadRequest(new { mensaje = "Hubo problemas al eliminar el registro." });
        }
    }
}
