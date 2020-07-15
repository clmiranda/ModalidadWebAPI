using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Usuario;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class UserService : IUserService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<User> Login(UserForLoginDto userforLogin)
        {
            var user = await _unitOfWork.UserRepository.FindByName(userforLogin.Username);
            if (user == null)
                return null;
            if (!await _unitOfWork.UserRepository.IsEmailConfirmed(user))
                return null;

            var result = await _unitOfWork.UserRepository.CheckPassword(user, userforLogin.Password, false);
            if (result.Succeeded)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        public async Task<object> GenerateJwtToken(User user, string security)
        {
            var claims = new List<Claim> { //claims for the token
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
            };

            var roles = await _unitOfWork.UserRepository.GetRoles(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(security));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); //credentials for the token

            //creating the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1), //tiempo de expiracion
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var appUser = _mapper.Map<UserForListDto>(user);

            //var x = new {nombre="asdfasdf",id=2,estado="activo" };

            return new { user = appUser, token = tokenHandler.WriteToken(token) };
        }
        public async Task<IdentityResult> RegisterUser(UserForRegisterDto userforRegisterDto)
        {
            var userToCreate = _mapper.Map<User>(userforRegisterDto);
            var result = await _unitOfWork.UserRepository.CreateUser(userToCreate, userforRegisterDto.Password);
            _unitOfWork.UserRepository.AddRole(userToCreate);
            return result;
        }
        public async Task<UserForDetailedDto> GetUsuario(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            if (user == null) return null;
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return userToReturn;
        }
        public async Task<IdentityResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return null;
            }
            var user = await _unitOfWork.UserRepository.FindById(userId);
            if (user == null)
            {
                return null;
            }
            var result = await _unitOfWork.UserRepository.ConfirmEmail(user, token);
            return result;
        }
        public async Task<string> ForgotPassword(ForgotPasswordDto objeto)
        {
            var user = await _unitOfWork.UserRepository.FindByEmail(objeto.Email);
            if (user != null)
            {
                if (await _unitOfWork.UserRepository.IsEmailConfirmed(user))
                {
                    return Base64UrlEncoder.Encode(await _unitOfWork.UserRepository.GeneratePasswordResetToken(user));
                }
                else return "ErrorEmail";
            }
            else return "ErrorUser";
        }
        public async Task<UserTokenToReturnDto> GetEmailToken(string email)
        {
            var user = await _unitOfWork.UserRepository.FindByEmail(email);
            var userToReturn = _mapper.Map<UserTokenToReturnDto>(user);
            userToReturn.Token = await _unitOfWork.UserRepository.GenerateEmailToken(user);

            return userToReturn;
        }
        public async Task<IdentityResult> ResetPassword(ResetPasswordDto reset)
        {
            reset.Token = Base64UrlEncoder.Decode(reset.Token);
            var user = await _unitOfWork.UserRepository.FindByEmail(reset.Email);
            if (user != null)
            {
                var result = await _unitOfWork.UserRepository.ResetPassword(user, reset.Token, reset.Password);
                return result;
            }
            else
                return null;
        }
    }
}
