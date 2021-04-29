using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using webapi.business.Dtos.Usuario;
using webapi.business.Services.Interf;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRolUserService _roleUserService;
        private readonly HttpContext _httpContext;
        public UserController(IUserService userService, IMapper mapper,
           IRolUserService roleUserService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _mapper = mapper;
            _roleUserService = roleUserService;
            _httpContext = httpContextAccessor.HttpContext;
        }
        [AllowAnonymous]
        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _userService.GetUsuario(id);
            if (user == null)
                return NotFound("El Usuario no existe.");
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }
        //[AllowAnonymous]
        //[HttpGet("GetUser")]
        //public async Task<ActionResult> GetUser()
        //{
        //    var user = await _userService.GetAll();
        //    var userToReturn = _mapper.Map<IEnumerable<UserRolesForReturn>>(user);
        //    return Ok(userToReturn);
        //}
        [AllowAnonymous]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(UserUpdateDto user)
        {
            if (user.Id!=int.Parse(_httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized(new { mensaje = "El id del usuario no existe." });
            var u = await _userService.GetUsuario(user.Id);
            if (u == null)
                return BadRequest(new { mensaje = "El Usuario no existe." });
            var resul = await _userService.UpdateUsuario(user);
            if (resul.Succeeded) {
                var userToReturn = _mapper.Map<UserForDetailedDto>(u);
                return Ok(userToReturn);
            }
            return BadRequest(new { mensaje = resul.Errors.FirstOrDefault().Description });
        }
        [AllowAnonymous]
        [HttpPut("ResetPassword/{id}")]
        public async Task<ActionResult> ResetPassword(int id, [FromBody]string password) {
            //if (id == int.Parse(_httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                var resul = await _userService.ResetPassword(id, password);
                if (resul.Succeeded)
                    return Ok();
                return BadRequest(new { mensaje = resul.Errors.FirstOrDefault().Description });
            //}
            //return BadRequest("Usuario Inválido.");
        }


        [Authorize(Roles = "SuperAdministrador")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var x = await _userService.GetAll();
            var lista = _mapper.Map<IEnumerable<UserRolesForReturn>>(x);
            return Ok(lista);
        }

        [Authorize(Roles = "SuperAdministrador")]
        [HttpPost("PutRolesUser/{id}")]
        public async Task<IActionResult> PutRolesUser(int id,  string[] RolesUsuario)
        {
            var userRoles = await _roleUserService.PutRolesUser(id, RolesUsuario);
            if (userRoles == null)
                return BadRequest(new { mensaje = "Error al editar Roles." });
            return Ok(userRoles);
        }
        [Authorize(Roles = "SuperAdministrador")]
        [HttpPut("CambiarEstado/{id}")]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var usuario = await _userService.GetUsuario(id);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario no existe." });
            var resultado = await _userService.CambiarEstado(id);
            if (resultado.Succeeded)
                return Ok(resultado);
            return BadRequest(new { mensaje = "Error al modificar estado." });
        }
        [Authorize(Roles = "SuperAdministrador")]
        [HttpDelete("EliminarUsuario/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _userService.GetUsuario(id);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario no existe." });
            var resultado = await _userService.EliminarUsuario(id);
            if (resultado.Succeeded)
                return Ok(resultado);
            return BadRequest(new { mensaje = "Error al eliminar." });
        }
    }
}