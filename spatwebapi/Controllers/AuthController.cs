using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public AuthController(IConfiguration config, IUserService userService,
             IEmailService emailService, UserManager<User> userManager)
        {
            _config = config;
            _userService = userService;
            _emailService = emailService;
            _userManager = userManager;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userService.FindUser(userForLoginDto);
            if (user == null)
                return Unauthorized(new { mensaje = "Error, debe confirmar su email o asegurarse de que los datos ingresados sean los correctos." });
            var token = await _userService.GenerateJwtToken(user, _config.GetSection("AppSettings:Token").Value);
            return Ok(token);
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string idUser, string token)
        {
            var result = await _userService.ConfirmEmail(idUser, token);
            if (result == null)
                return BadRequest("Ha ocurrido un error, Id o Token inválido.");

            if (result.Succeeded)
                return Redirect("https://localhost:44363/LoginAccount/ConfirmEmail");
            else
                return BadRequest(new { mensaje = result.Errors.FirstOrDefault().Description });
        }
        [HttpPost("ForgotPassword")]
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
                        new { email = email.Email, token }, Request.Scheme);

                await _emailService.SendEmailAsync(email.Email, "Enlace para reestablecer la contraseña de la cuenta en el sitio web de S.P.A.T.", "<a href=" + linkReseteo + "><h3>Accede a este enlace para reestablecer tu contraseña.</h3></a>");
                return Ok();
            }
        }
        [HttpGet("ValidateResetPassword")]
        public RedirectResult ValidateResetPassword(string email, string token)
        {
            return Redirect("https://localhost:44363/LoginAccount/ResetPassword?email=" + email + "&token=" + token);
        }
        [HttpPost("ResetPasswordExterno")]
        public async Task<IActionResult> ResetPasswordExterno(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return NotFound(new { mensaje = "El email no se encuentra registrado." });
            var result = await _userService.ResetPasswordExterno(resetPasswordDto);
            if (result.Succeeded)
                return Ok();
            else return BadRequest(new { mensaje = result.Errors.FirstOrDefault().Description });
        }
    }
}