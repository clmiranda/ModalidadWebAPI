using System;
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
        private readonly IEmailService _emailService;
        public SeguimientoController(ISeguimientoService seguimientoService, IMapper mapper,
            IEmailService emailService)
        {
            _seguimientoService = seguimientoService;
            _mapper = mapper;
            _emailService = emailService;
        }
        [HttpGet("GetAllSeguimientosForReport")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> GetAllSeguimientosForReport()
        {
            var lista = await _seguimientoService.GetAllSeguimientosForReport();
            var mapped = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(lista);
            return Ok(mapped);
        }
        [HttpGet("GetAllSeguimiento")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> GetAllSeguimiento([FromQuery] SeguimientoParametros parametros)
        {
            var lista = await _seguimientoService.GetAllSeguimiento(parametros);
            var mapped = _mapper.Map<IEnumerable<SeguimientoForReturnDto>>(lista.ToList());
            mapped = mapped.OrderByDescending(x => x.Id).ToList();
            Response.AddPagination(lista.CurrentPage, lista.PageSize,
                 lista.TotalCount, lista.TotalPages);
            return Ok(mapped);
        }
        [HttpGet("GetAllSeguimientoVoluntario")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> GetAllSeguimientoVoluntario([FromQuery] SeguimientoParametros parametros)
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
        public async Task<IActionResult> GetAllVoluntarios()
        {
            var listaUsers = await _seguimientoService.GetAllVoluntarios();
            var mapped = _mapper.Map<IEnumerable<UserForDetailedDto>>(listaUsers);
            return Ok(mapped);
        }
        [HttpGet("GetSeguimiento/{id}")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> GetSeguimiento(int id)
        {
            var seguimiento = await _seguimientoService.GetById(id);
            if (seguimiento == null)
                return Ok(null);
            seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderBy(x => x.FechaReporte).ToList().OrderBy(x => x.Estado).ToList();
            var mapped = _mapper.Map<SeguimientoForReturnDto>(seguimiento);
            return Ok(mapped);
        }
        [HttpGet("GetSeguimientoForVoluntario/{id}")]
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        public async Task<IActionResult> GetSeguimientoForVoluntario(int id)
        {
            var seguimiento = await _seguimientoService.GetById(id);
            if (seguimiento == null)
                return Ok(null);
            var idUser = int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
            if (seguimiento.UserId == idUser)
            {
                seguimiento.ReporteSeguimientos = seguimiento.ReporteSeguimientos.OrderBy(x => x.FechaReporte.Date).ToList().OrderByDescending(x => x.FechaReporte.ToShortDateString().Equals(DateTime.Now.ToShortDateString())).ToList().FindAll(x => !x.Estado.Equals("Activo")).ToList();
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
            if (resultado) {
                var seguimiento = _mapper.Map<SeguimientoForReturnDto>(await _seguimientoService.GetById(id));
                var linkSeguimiento = Url.Action("SeguimientoAsignado", "Seguimiento",
                        new { id }, Request.Scheme);
                await _emailService.SendEmailAsync(seguimiento.User.Email, $"Hola {seguimiento.User.Persona.Nombres}, has sido asignado(a) al seguimiento de la mascota {seguimiento.SolicitudAdopcion.Mascota.Nombre}.", "<a href=" + linkSeguimiento + "><h3>Accede a este enlace para ver la información del seguimiento.</h3></a>");
                return Ok();
            }
            return BadRequest(new { mensaje = "Problemas al guardar los datos." });
        }
        [AllowAnonymous]
        [HttpGet("SeguimientoAsignado")]
        public RedirectResult SeguimientoAsignado(int id)
        {
            return Redirect("https://localhost:44363/SeguimientoAsignado/ListaReportes/" + id);
        }
        [HttpPut("{id}/QuitarAsignacion/{idUser}")]
        [Authorize(Roles = "SuperAdministrador, Administrador")]
        public async Task<IActionResult> QuitarAsignacion(int id, int idUser)
        {
            var resultado = await _seguimientoService.QuitarAsignacion(id, idUser);
            if (resultado)
            {
                var listaUsers = await _seguimientoService.GetAllVoluntarios();
                var mapped = _mapper.Map<IEnumerable<UserForDetailedDto>>(listaUsers);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = "Problemas al guardar los datos." });
        }
    }
}