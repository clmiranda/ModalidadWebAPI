using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Usuario;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IUserService
    {
        IEnumerable<User> GetAllVoluntarios();
        Task<User> Login(UserForLoginDto userforLogin);
        Task<object> GenerateJwtToken(User user, string security);
        Task<IdentityResult> RegisterUser(UserForRegisterDto user); //dto
        Task<UserTokenToReturnDto> GetEmailToken(string email);
        Task<IdentityResult> ConfirmEmail(string userId, string token);
        Task<string> ForgotPassword(ForgotPasswordDto email);
        Task<IdentityResult> ResetPassword(ResetPasswordDto reset);
        Task<UserForDetailedDto> GetUsuario(int id);
        //Task<bool> UpdateUser(int id, UserForUpdateDto userForUpdateDto);
        //Task<PagedList<User>> GetUsers(UserParams userParams);
    }
}
