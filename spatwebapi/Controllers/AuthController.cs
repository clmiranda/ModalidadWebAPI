using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using webapi.business.Dtos.Usuario;
using webapi.business.Services.Interf;
using webapi.core.Models;

namespace spatwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private IUserService _userService;
        private readonly IEmailService _emailService;
        private IMapper _mapper;
        public AuthController(IConfiguration config, IUserService userService,
            IMapper mapper, IEmailService emailService, UserManager<User> userManager)
        {
            _config = config;
            _userService = userService;
            _mapper = mapper;
            _emailService = emailService;
            _userManager = userManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userforLogin)
        {
            //throw new Exception(message:"Error Interno del Servidor.");
            var user = await _userService.Login(userforLogin);
            if (user == null)
                return Unauthorized(new { mensaje = "Error, debe confirmar su email o asegurarse de que los datos ingresados sean los correctos." });
            var aux = await _userService.GenerateJwtToken(user, _config.GetSection("AppSettings:Token").Value);

            return Ok(aux);
        }
        [HttpPost("PostUsuario")]
        public async Task<IActionResult> PostUsuario([FromBody]UserForRegisterDto userforRegisterDto)
        {
            var result = await _userService.PostUsuario(userforRegisterDto);
            if (result.Succeeded)
            {
                var userToCreate = await _userService.GetEmailToken(userforRegisterDto.Email);
                var confirmationLink = Url.Action("ConfirmEmail", "Auth",
                    new { userId = userToCreate.Id, token = userToCreate.Token }, Request.Scheme);
                await _emailService.SendEmailAsync(userforRegisterDto.Email, "Enlace de Confirmacion para la cuenta en el sitio web de S.P.A.T.", "<a href="+ confirmationLink + "><h5>Accede a este enlace para poder confirmar tu correo electrónico en el sitio web de S.P.A.T.</h5></a>");
                //string subject = "Enlace de Confirmacion para la cuenta en el sitio web de S.P.A.T.";
                //string body = "Accede a este enlace para poder confirmar tu correo electrónico en el sitio web de S.P.A.T.";
                //SendEmail.SendEmailConfirmation(subject, body, confirmationLink, userforRegisterDto.Email);
                return Ok();
            }
            else
                return BadRequest(new { mensaje=result.Errors.FirstOrDefault().Description });
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _userService.ConfirmEmail(userId, token);
            if (result == null)
                return BadRequest("Ha ocurrido un error, Id o Token inválido.");

            if (result.Succeeded)
                return Redirect("https://localhost:44363/Cuenta/ConfirmEmail");
            else
                return BadRequest(new { mensaje = result.Errors.FirstOrDefault().Description });
        }
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto email)
        {
            var token = await _userService.ForgotPassword(email);
            if (token.Equals("ErrorUser"))
                return BadRequest(new { mensaje = "El email ingresado no ha sido registrado en el sistema." });
            else if (token.Equals("ErrorEmail"))
                return BadRequest(new { mensaje = "El email ingresado aún no ha sido verificado." });
            else
            {
                var linkReseteo = Url.Action("ValidateResetPassword", "Auth",
                        new { email = email.Email, token = token }, Request.Scheme);

                //string subject = "Enlace para reestablecer la contraseña de la cuenta en el sitio web de S.P.A.T.";
                //string body = "Accede a este enlace para poder reestablecer tu contraseña.";
                //SendEmail.SendEmailConfirmation(subject, body, linkReseteo, email.Email);
                await _emailService.SendEmailAsync(email.Email, "Enlace para reestablecer la contraseña de la cuenta en el sitio web de S.P.A.T.", "<a href=" + linkReseteo + "><h5>Accede a este enlace para reestablecer tu contraseña.</h5></a>");
                return Ok();
            }
        }
        [HttpGet("ValidateResetPassword")]
        public RedirectResult ValidateResetPassword(string email, string token)
        {
            return Redirect("https://localhost:44363/Cuenta/ResetPassword?email=" + email + "&token=" + token);
        }
        [HttpPost("ResetPasswordExterno")]
        public async Task<IActionResult> ResetPasswordExterno(ResetPasswordDto reset)
        {
            var user = await _userManager.FindByEmailAsync(reset.Email);
            if (user == null)
                return NotFound(new { mensaje = "El email no se encuentra registrado." });
            var result = await _userService.ResetPasswordExterno(reset);
            //if (reset == null)
            //    return BadRequest(new { mensaje = "El Usuario o Token es inválido." });
            if (result.Succeeded)
                return Ok();
            else return BadRequest(new { mensaje = result.Errors.FirstOrDefault().Description });
        }
    }
}