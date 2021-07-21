using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(Roles = "SuperAdministrador")]
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
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _userService.GetUsuario(id);
            if (user == null)
                return NotFound("El Usuario no existe.");
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(UserUpdateDto userDto)
        {
            if (userDto.Id!=int.Parse(_httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized(new { mensaje = "El id del usuario no existe." });
            var usuario = await _userService.GetUsuario(userDto.Id);
            if (usuario == null)
                return BadRequest(new { mensaje = "El Usuario no existe." });
            var resul = await _userService.UpdateUsuario(userDto);
            if (resul.Succeeded) {
                var mapped = _mapper.Map<UserForDetailedDto>(usuario);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = resul.Errors.FirstOrDefault().Description });
        }
        [HttpPut("UpdateEmail")]
        public async Task<ActionResult> UpdateEmail(UserUpdateDto userDto)
        {
            if (userDto.Id != int.Parse(_httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized(new { mensaje = "El id del usuario no existe." });
            var usuario = await _userService.GetUsuario(userDto.Id);
            if (usuario == null)
                return BadRequest(new { mensaje = "El Usuario no existe." });
            var resul = await _userService.UpdateEmail(userDto);
            if (resul.Succeeded)
            {
                var mapped = _mapper.Map<UserForDetailedDto>(usuario);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = resul.Errors.FirstOrDefault().Description });
        }
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        [HttpPut("ResetPassword/{id}")]
        public async Task<ActionResult> ResetPassword(int id, [FromBody]string password) {
                var resul = await _userService.ResetPassword(id, password);
                if (resul.Succeeded)
                    return Ok();
                return BadRequest(new { mensaje = resul.Errors.FirstOrDefault().Description });
        }
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var listaUsers = await _userService.GetAll();
            var mapped = _mapper.Map<IEnumerable<UserRolesForReturn>>(listaUsers);
            return Ok(mapped);
        }
        [HttpPost("PutRolesUser/{id}")]
        public async Task<IActionResult> PutRolesUser(int id,  string[] RolesUsuario)
        {
            var userRoles = await _roleUserService.PutRolesUser(id, RolesUsuario);
            if (userRoles == null)
                return BadRequest(new { mensaje = "Error al editar Roles." });
            return Ok(userRoles);
        }
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
        [HttpDelete("EliminarUsuario/{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var usuario = await _userService.GetUsuario(id);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario no existe." });
            var resultado = await _userService.DeleteUsuario(id);
            if (resultado.Succeeded)
                return Ok(resultado);
            return BadRequest(new { mensaje = "Error al eliminar." });
        }
    }
}