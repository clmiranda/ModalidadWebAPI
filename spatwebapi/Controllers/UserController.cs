﻿using AutoMapper;
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
        private readonly IEmailService _emailService;
        public UserController(IUserService userService, IMapper mapper,
           IRolUserService roleUserService, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _userService = userService;
            _mapper = mapper;
            _roleUserService = roleUserService;
            _httpContext = httpContextAccessor.HttpContext;
            _emailService = emailService;
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
        [Authorize(Roles = "SuperAdministrador")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserForRegisterDto userforRegisterDto)
        {
            var result = await _userService.CreateUser(userforRegisterDto);
            if (result.Succeeded)
            {
                var userToCreate = await _userService.GetEmailToken(userforRegisterDto.Email);
                var confirmationLink = Url.Action("ConfirmEmail", "Auth",
                    new { userId = userToCreate.Id, token = userToCreate.Token }, Request.Scheme);

                await _emailService.SendEmailAsync(userforRegisterDto.Email, "Enlace de Confirmacion para la cuenta en el sitio web de S.P.A.T.", "<a href=" + confirmationLink + "><h5>Accede a este enlace para poder confirmar tu correo electrónico en el sitio web de S.P.A.T.</h5></a>");
                return Ok();
            }
            else
                return BadRequest(new { mensaje = result.Errors.FirstOrDefault().Description });
        }
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
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
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        [HttpPut("UpdateEmail")]
        public async Task<ActionResult> UpdateEmail(UpdateEmailDto updateEmailDto)
        {
            if (updateEmailDto.Id != int.Parse(_httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)) return Unauthorized(new { mensaje = "El id del usuario no existe." });
            var usuario = await _userService.GetUsuario(updateEmailDto.Id);
            if (usuario == null)
                return BadRequest(new { mensaje = "El Usuario no existe." });
            var resul = await _userService.UpdateEmail(updateEmailDto);
            if (resul.Succeeded)
            {
                var mapped = _mapper.Map<UserForDetailedDto>(usuario);
                return Ok(mapped);
            }
            return BadRequest(new { mensaje = resul.Errors.FirstOrDefault().Description });
        }
        [Authorize(Roles = "SuperAdministrador, Administrador, Voluntario")]
        [HttpPut("ResetPassword/{id}")]
        public async Task<ActionResult> ResetPassword(int id, UpdateUserPassword dto) {
                var resul = await _userService.ResetPassword(id, dto.Password);
                if (resul.Succeeded)
                    return Ok();
                return BadRequest(new { mensaje = resul.Errors.FirstOrDefault().Description });
        }
        [Authorize(Roles = "SuperAdministrador")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var listaUsers = await _userService.GetAllUsers();
            var mapped = _mapper.Map<IEnumerable<UserRolesForReturn>>(listaUsers);
            return Ok(mapped);
        }
        [Authorize(Roles = "SuperAdministrador")]
        [HttpPost("AsignarRoles/{id}")]
        public async Task<IActionResult> AsignarRoles(int id,  string[] rolesUser)
        {
            var userRoles = await _roleUserService.AsignarRoles(id, rolesUser);
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
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var usuario = await _userService.GetUsuario(id);
            if (usuario == null)
                return BadRequest(new { mensaje = "El usuario no existe." });
            var resultado = await _userService.DeleteUser(id);
            if (resultado.Succeeded)
                return Ok(resultado);
            return BadRequest(new { mensaje = "Error al eliminar." });
        }
    }
}