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
                return BadRequest("El Usuario no existe.");
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }
        [AllowAnonymous]
        [HttpGet("GetUser")]
        public ActionResult GetUser()
        {
            var user = _userService.GetAll();
            var userToReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(user);
            return Ok(userToReturn);
        }
        [AllowAnonymous]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(UserUpdateDto user)
        {
            if (user.Id!=int.Parse(_httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized();
            var u = await _userService.GetUsuario(user.Id);
            if (u == null)
                return BadRequest("El Usuario no existe.");
            var resul = await _userService.UpdateUsuario(user);
            if (resul.Succeeded) {
                var userToReturn = _mapper.Map<UserForDetailedDto>(u);
                return Ok(userToReturn);
            }
            return BadRequest(resul.Errors);
        }
        [AllowAnonymous]
        [HttpPut("ResetPassword/{id}")]
        public async Task<ActionResult> ResetPassword(int id, [FromBody]string password) {
            if (id == int.Parse(_httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)) {
                var resul = await _userService.ResetPassword(id, password);
                if (resul.Succeeded)
                    return Ok();
                return BadRequest(resul.Errors);
            }
            return BadRequest("Usuario Inválido.");
            //var id = int.Parse(_httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //var resul = await _userService.ResetPassword();
        }


        [Authorize(Roles = "SuperAdministrador")]
        [HttpGet("GetRolesUsuarios")]
        public async Task<IActionResult> GetRolesUsuarios()
        {
            var x = await _userService.GetRolesUsuarios();
            var lista = _mapper.Map<IEnumerable<UserRolesForReturn>>(x);
            return Ok(lista);
        }

        [Authorize(Roles = "SuperAdministrador")]
        [HttpPost("PutRolesUser/{userName}")]
        public async Task<IActionResult> PutRolesUser(string userName,  string[] RolesUsuario)
        {
            var userRoles = await _roleUserService.PutRolesUser(userName, RolesUsuario);
            if (userRoles == null)
                return BadRequest("Error al editar Roles");
            return Ok(userRoles);
        }
    }
}
