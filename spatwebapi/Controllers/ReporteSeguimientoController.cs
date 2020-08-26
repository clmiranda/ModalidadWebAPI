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
        private IUserService _userService;
        private readonly IMapper _mapper;
        public ReporteSeguimientoController(IReporteSeguimientoService reporteSeguimientoService, IMapper mapper,
            IUserService userService)
        {
            _reporteSeguimientoService = reporteSeguimientoService;
            _mapper = mapper;
            _userService = userService;
        }
        [HttpGet("ListReporteSeguimiento")]
        public async Task<IEnumerable<ReporteSeguimientoForList>> ListReporteSeguimiento()
        {
            return _mapper.Map<IEnumerable<ReporteSeguimientoForList>>(await _reporteSeguimientoService.GetAll());
        }
        [HttpGet("GetAllVoluntarios")]
        public IEnumerable<UserForListDto> GetAllVoluntarios()
        {
            var lista = _userService.GetAllVoluntarios();
            return _mapper.Map<IEnumerable<UserForListDto>>(lista);
        }
        [HttpPost("CreateReporteSeguimiento")]
        public async Task<IActionResult> CreateReporteSeguimiento(ReporteSeguimientoForCreate reporteDto)
        {
            if (await _reporteSeguimientoService.CreateReporteSeguimiento(reporteDto))
                return Ok("El Reporte fue agregado exitosamente.");

            return BadRequest("Hubo problemas al agregar el Reporte");
        }
    }
}