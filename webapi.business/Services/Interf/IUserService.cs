using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.business.Dtos.Usuario;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IUserService
    {
        Task<User> Login(UserForLoginDto userForLoginDto);
        Task<object> GenerateJwtToken(User user, string security);
        Task<IdentityResult> PostUsuario(UserForRegisterDto userforRegisterDto);
        Task<IdentityResult> UpdateUsuario(UserUpdateDto userForUpdateDto);
        Task<IdentityResult> UpdateEmail(UpdateEmailDto userForUpdateDto);
        Task<UserTokenToReturnDto> GetEmailToken(string email);
        Task<IdentityResult> ConfirmEmail(string userId, string token);
        Task<string> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Task<IdentityResult> ResetPassword(int id, string password);
        Task<IdentityResult> ResetPasswordExterno(ResetPasswordDto resetPasswordDto);
        Task<User> GetUsuario(int id);
        Task<IdentityResult> CambiarEstado(int id);
        Task<IdentityResult> DeleteUsuario(int id);
        Task<IEnumerable<User>> GetAll();
    }
}
