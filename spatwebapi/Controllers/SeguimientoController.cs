using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Dtos.Usuario;
using webapi.business.Helpers;
using webapi.business.Pagination;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

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
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var lista = _seguimientoService.GetAll();
            var mapeado = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(lista);
            return Ok(mapeado);
        }
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        [HttpGet("GetAllSeguimiento")]
        public async Task<ActionResult> GetAllSeguimiento([FromQuery] SeguimientoParametros parametros)
        {
            var resul = await _seguimientoService.GetAllSeguimiento(parametros);
            var lista = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(resul.ToList());
            Response.AddPagination(resul.CurrentPage, resul.PageSize,
                 resul.TotalCount, resul.TotalPages);
            return Ok(lista);
        }
        [HttpGet("GetAllVoluntarios")]
        public IEnumerable<UserForDetailedDto> GetAllVoluntarios()
        {
            var lista = _seguimientoService.GetAllVoluntarios();
            var mapped = _mapper.Map<IEnumerable<UserForDetailedDto>>(lista);
            return mapped;
        }
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        [HttpGet("GetSeguimiento/{id}")]
        public async Task<ActionResult> GetSeguimiento(int id)
        {
            var seguimiento = await _seguimientoService.GetById(id);
            if (seguimiento == null)
                return NotFound(null);
            var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);

            //mapped.ReporteSeguimientos = mapped.ReporteSeguimientos.OrderByDescending(x => x.Fecha.ToShortDateString().Equals(DateTime.Now.ToShortDateString())).ThenBy(x => x.Fecha.Date).ToList();
            return Ok(mapped);
        }
        [Authorize(Roles = "SuperAdministrador, Voluntario")]
        [HttpGet("GetSeguimientoForVoluntario/{id}")]
        public async Task<ActionResult> GetSeguimientoForVoluntario(int id)
        {
            var seguimiento = await _seguimientoService.GetById(id);
            if (seguimiento == null)
                return NotFound(null);
            if (seguimiento.User == null)
                return NotFound(null);
            var seg = _mapper.Map<SeguimientoForReturnDto>(seguimiento);

            seg.ReporteSeguimientos = seg.ReporteSeguimientos.Where(x => x.Fecha.Date >= seg.FechaInicio.Date && x.Fecha.Date <= seg.FechaConclusion.Date).Where(x => !x.Estado.Equals("Enviado")).ToList();
            return Ok(seg);
        }
        //talvez no se usa esta action
        //[AllowAnonymous]
        //[HttpPut("SaveSeguimiento")]
        //public async Task<IActionResult> SaveSeguimiento([FromBody] Seguimiento seguimiento)
        //{
        //    if (await _seguimientoService.UpdateSeguimiento(seguimiento)) {
        //        return Ok("Datos actualizados correctamente, se generaron los reportes respectivos.");
        //    }
        //    return BadRequest("Ocurrio un problema al actualizar los datos.");
        //}
        [AllowAnonymous]
        [HttpPut("UpdateFecha")]
        public async Task<IActionResult> UpdateFecha([FromBody] FechaReporteDto dto)
        {
            var seguimiento = await _seguimientoService.GetById(dto.Id);
            if (seguimiento != null)
            {
                var resultado = await _seguimientoService.UpdateFecha(dto);
                if (resultado != null)
                {
                    var mapped = _mapper.Map<SeguimientoForReturnDto>(resultado);
                    return Ok(mapped);
                }
                return BadRequest(new { mensaje = "Ocurrio un problema al actualizar los datos." });
            }
            return BadRequest(new { mensaje = "No existe el Seguimiento." });
        }
        [AllowAnonymous]
        [HttpPut("{id}/AsignarSeguimiento/{idUser}")]
        public async Task<IActionResult> AsignarSeguimiento(int id, int idUser)
        {
            var resultado = await _seguimientoService.AsignarSeguimiento(id, idUser);
            if (resultado)
                return Ok();
            return BadRequest(new { mensaje = "Ocurrio un problema al guardar los datos." });
        }
        [AllowAnonymous]
        [HttpPut("{id}/QuitarAsignacion/{idUser}")]
        public async Task<IActionResult> QuitarAsignacion(int id, int idUser)
        {
            if (await _seguimientoService.QuitarAsignacion(id, idUser))
            {
                var voluntarios = _seguimientoService.GetAllVoluntarios();
                var mapper = _mapper.Map<IEnumerable<UserForDetailedDto>>(voluntarios);
                return Ok(mapper);
            }
            return BadRequest(new { mensaje = "Ocurrio un problema al guardar los datos." });
        }

        [Authorize(Roles = "Voluntario")]
        [HttpPost("{id}/AceptarSeguimientoVoluntario")]
        public async Task<IActionResult> AceptarSeguimientoVoluntario(int id)
        {
            var resul = await _seguimientoService.AceptarSeguimientoVoluntario(id);
            if (resul)
                return Ok();
            return BadRequest(new { mensaje = "Hubo un problema al asignar el seguimiento." });
        }
        [Authorize(Roles = "Voluntario")]
        [HttpPost("{id}/RechazarSeguimientoVoluntario")]
        public async Task<IActionResult> RechazarSeguimientoVoluntario(int id)
        {
            var resul = await _seguimientoService.RechazarSeguimientoVoluntario(id);
            if (resul)
                return Ok();
            return BadRequest(new { mensaje = "Hubo un problema al rechazar el seguimiento." });
        }
    }
}