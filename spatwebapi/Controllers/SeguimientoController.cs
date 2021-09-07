using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Dtos.Usuario;
using webapi.business.Helpers;
using webapi.business.Pagination;
using webapi.business.Services.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguimientoController : ControllerBase
    {
        private readonly ISeguimientoService _seguimientoService;
        private readonly IMapper _mapper;
        public SeguimientoController(ISeguimientoService seguimientoService, IMapper mapper)
        {
            _seguimientoService = seguimientoService;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public IActionResult GetAll()
        {
            var lista = _seguimientoService.GetAll();
            var mapped = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetAllSeguimiento")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<ActionResult> GetAllSeguimiento([FromQuery] SeguimientoParametros parametros)
        {
            var lista = await _seguimientoService.GetAllSeguimiento(parametros);
            var mapped = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(lista.ToList());
            Response.AddPagination(lista.CurrentPage, lista.PageSize,
                 lista.TotalCount, lista.TotalPages);
            return Ok(mapped);
        }
        [HttpGet("GetAllVoluntarios")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public IEnumerable<UserForDetailedDto> GetAllVoluntarios()
        {
            var lista = _seguimientoService.GetAllVoluntarios();
            var mapped = _mapper.Map<IEnumerable<UserForDetailedDto>>(lista);
            return mapped;
        }
        [HttpGet("GetSeguimiento/{id}")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<ActionResult> GetSeguimiento(int id)
        {
            var seguimiento = await _seguimientoService.GetById(id);
            if (seguimiento == null)
                return NotFound(null);
            seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderByDescending(x => x.Fecha).ToList();
            var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
            return Ok(mapped);
        }
        [HttpPut("UpdateFecha")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> UpdateFecha([FromBody] FechaReporteDto dto)
        {
            var seguimiento = await _seguimientoService.GetById(dto.Id);
            if (seguimiento != null)
            {
                var resultado = await _seguimientoService.UpdateFecha(dto);
                if (resultado != null)
                {
                    var mapped = _mapper.Map<SeguimientoForReturnDto>(resultado);
                    mapped.ReporteSeguimientos = mapped.ReporteSeguimientos.OrderByDescending(x => x.Fecha).ToList();
                    return Ok(mapped);
                }
                return BadRequest(new { mensaje = "Problemas al actualizar los datos." });
            }
            return BadRequest(new { mensaje = "No existe el Seguimiento." });
        }
        [HttpPut("{id}/AsignarSeguimiento/{idUser}")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> AsignarSeguimiento(int id, int idUser)
        {
            var resultado = await _seguimientoService.AsignarSeguimiento(id, idUser);
            if (resultado)
                return Ok();
            return BadRequest(new { mensaje = "Problemas al guardar los datos." });
        }
        [HttpPut("{id}/QuitarAsignacion/{idUser}")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> QuitarAsignacion(int id, int idUser)
        {
            var resultado = await _seguimientoService.QuitarAsignacion(id, idUser);
            if (resultado)
            {
                var lista = _seguimientoService.GetAllVoluntarios();
                var mapper = _mapper.Map<IEnumerable<UserForDetailedDto>>(lista);
                return Ok(mapper);
            }
            return BadRequest(new { mensaje = "Problemas al guardar los datos." });
        }

        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        [HttpPost("{id}/AceptarSeguimientoVoluntario")]
        public async Task<IActionResult> AceptarSeguimientoVoluntario(int id)
        {
            var resultado = await _seguimientoService.AceptarSeguimientoVoluntario(id);
            if (resultado)
                return Ok();
            return BadRequest(new { mensaje = "Problemas al asignar el seguimiento." });
        }
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        [HttpPost("{id}/RechazarSeguimientoVoluntario")]
        public async Task<IActionResult> RechazarSeguimientoVoluntario(int id)
        {
            var resultado = await _seguimientoService.RechazarSeguimientoVoluntario(id);
            if (resultado)
                return Ok();
            return BadRequest(new { mensaje = "Problemas al rechazar el seguimiento." });
        }
    }
}