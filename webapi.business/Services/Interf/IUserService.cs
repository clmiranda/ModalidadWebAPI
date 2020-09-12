using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Usuario;
using webapi.business.Helpers;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IUserService
    {
        Task<PaginationList<User>> GetAllVoluntarios(VoluntarioParameters voluntarioParameters);
        Task<User> Login(UserForLoginDto userforLogin);
        Task<object> GenerateJwtToken(User user, string security);
        Task<IdentityResult> RegisterUser(UserForRegisterDto user); //dto
        Task<UserTokenToReturnDto> GetEmailToken(string email);
        Task<IdentityResult> ConfirmEmail(string userId, string token);
        Task<string> ForgotPassword(ForgotPasswordDto email);
        Task<IdentityResult> ResetPassword(ResetPasswordDto reset);
        Task<User> GetUsuario(int id);
        //Task<bool> UpdateUser(int id, UserForUpdateDto userForUpdateDto);
        //Task<PagedList<User>> GetUsers(UserParams userParams);
    }
}
