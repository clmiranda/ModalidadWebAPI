using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [Authorize(Roles = "SuperAdministrador, Administrador")]
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
        [HttpGet("GetAllSeguimientoVoluntario")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<ActionResult> GetAllSeguimientoVoluntario([FromQuery] SeguimientoParametros parametros)
        {
            var idUser = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            var lista = await _seguimientoService.GetAllSeguimientoVoluntario(idUser, parametros);
            var mapped = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(lista);
            Response.AddPagination(lista.CurrentPage, lista.PageSize,
                 lista.TotalCount, lista.TotalPages);
            return Ok(mapped);
        }
        [HttpGet("GetAllVoluntarios")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public IEnumerable<UserForDetailedDto> GetAllVoluntarios()
        {
            var listaUsers = _seguimientoService.GetAllVoluntarios();
            var mapped = _mapper.Map<IEnumerable<UserForDetailedDto>>(listaUsers);
            return mapped;
        }
        [HttpGet("GetSeguimiento/{id}")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<ActionResult> GetSeguimiento(int id)
        {
            var seguimiento = await _seguimientoService.GetById(id);
            if (seguimiento == null)
                return Ok(null);
            seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderByDescending(x => x.FechaReporte).ToList().OrderBy(x => x.Estado).ToList();
            var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
            return Ok(mapped);
        }
        [HttpGet("GetSeguimientoVoluntario/{id}")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<ActionResult> GetSeguimientoVoluntario(int id)
        {
            var seguimiento = await _seguimientoService.GetById(id);
            if (seguimiento == null)
                return Ok(null);
            var idUser = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            if (seguimiento.UserId == idUser)
            {
                seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderByDescending(x => x.FechaReporte.Date).ToList().OrderBy(x => x.Estado).ToList().FindAll(x => !x.Estado.Equals("Activo")).ToList();
                var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
                return Ok(mapped);
            }
            return Unauthorized();
        }
        [HttpPut("{id}/AsignarSeguimiento/{idUser}")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> AsignarSeguimiento(int id, int idUser)
        {
            var resultado = await _seguimientoService.AsignarSeguimiento(id, idUser);
            if (resultado)
                return Ok();
            return BadRequest(new { mensaje = "Problemas al guardar los datos." });
        }
        [HttpPut("{id}/QuitarAsignacion/{idUser}")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> QuitarAsignacion(int id, int idUser)
        {
            var resultado = await _seguimientoService.QuitarAsignacion(id, idUser);
            if (resultado)
            {
                var listaUsers = _seguimientoService.GetAllVoluntarios();
                var mapped = _mapper.Map<IEnumerable<UserForDetailedDto>>(listaUsers);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "Problemas al guardar los datos." });
        }
    }
}