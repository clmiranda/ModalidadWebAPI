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
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguimientoController : ControllerBase
    {
        private ISeguimientoService _seguimientoService;
        private IReporteSeguimientoService _reporteSeguimientoService;
        private IUserService _userService;
        private readonly IMapper _mapper;
        public SeguimientoController(ISeguimientoService seguimientoService, IMapper mapper,
            IUserService userService, IReporteSeguimientoService reporteSeguimientoService)
        {
            _seguimientoService = seguimientoService;
            _userService = userService;
            _reporteSeguimientoService = reporteSeguimientoService;
            _mapper = mapper;
        }
        [AllowAnonymous]
        [HttpGet("ListSeguimiento")]
        public async Task<IEnumerable<SeguimientoForReturnDto>> ListSeguimiento() {
            var lista= _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(await _seguimientoService.GetAll());
            return lista;
        }
        [HttpGet("GetAllVoluntarios")]
        public async Task<IActionResult> GetAllVoluntarios([FromQuery] VoluntarioParameters voluntarioParameters)
        {
            var lista = await _userService.GetAllVoluntarios(voluntarioParameters);
            var listaToReturn = _mapper.Map<IEnumerable<UserForListDto>>(lista);

            Response.AddPagination(lista.CurrentPage, lista.PageSize, lista.TotalCount, lista.TotalPages);
            return Ok(listaToReturn);
        }
        [AllowAnonymous]
        [HttpGet("GetSeguimiento/{id}")]
        public async Task<ActionResult<SeguimientoForReturnDto>> GetSeguimiento(int id)
        {
            var aux = await _seguimientoService.GetById(id);
            if (aux==null)
                return NotFound("Seguimiento no encontrado.");
            return Ok( _mapper.Map<SeguimientoForReturnDto>(aux));
        }
        [AllowAnonymous]
        [HttpPut("SaveSeguimiento")]
        public async Task<IActionResult> SaveSeguimiento([FromBody] Seguimiento modelo)
        {
            var objeto = await _seguimientoService.GetById(modelo.Id);
            objeto.CantidadVisitas = modelo.CantidadVisitas;
            if (await _seguimientoService.UpdateSeguimiento(objeto)) {
                return Ok("Datos actualizados correctamente, se generaron los reportes respectivos.");
            }
            return BadRequest("Ocurrio un problema al actualizar los datos.");
        }
        [AllowAnonymous]
        [HttpPut("{id}/User/{idUser}")]
        public async Task<IActionResult> CheckedVoluntarioAsignado(int id, int idUser)
        {
            var seguimiento = await _seguimientoService.GetById(id);
            var user = await _userService.GetUsuario(idUser);
            if (await _seguimientoService.CheckedVoluntarioAsignado(seguimiento, user))
                return Ok("La solicitud ha sido enviada, el Voluntario deberá aceptarla o rechazarla.");
            return BadRequest("Ocurrio un problema.");
        }


        [Authorize(Roles ="Voluntario")]
        [HttpGet("ListVoluntarioSeguimientos")]
        public IEnumerable<SeguimientoForReturnDto> ListVoluntarioSeguimientos()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var lista = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(_seguimientoService.FindByCondition(int.Parse(id)));
            return lista;
        }
        [Authorize(Roles = "Voluntario")]
        [HttpPost("{id}/AsignarSeguimiento")]
        public async Task<IActionResult> AsignarSeguimiento(int id)
        {
            var resul = await _seguimientoService.AsignarSeguimiento(id);
            if (resul)
                return Ok("Se ha asignado el seguimiento.");

            return BadRequest("Huno un problema al asignar el seguimiento.");
        }
        [Authorize(Roles = "Voluntario")]
        [HttpPost("{id}/RechazarSeguimiento")]
        public async Task<IActionResult> RechazarSeguimiento(int id)
        {
            var resul = await _seguimientoService.RechazarSeguimiento(id);
            if (resul)
                return Ok("Se ha rechazado el seguimiento.");

            return BadRequest("Huno un problema al rechazar el seguimiento.");
        }
    }
}