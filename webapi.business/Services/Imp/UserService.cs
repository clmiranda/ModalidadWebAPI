﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISeguimientoService _seguimientoService;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
            ISeguimientoService seguimientoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _seguimientoService = seguimientoService;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var users = await _unitOfWork.UserRepository.FindByCondition(x => x.UserRoles.All(y => !y.Role.Name.Equals("SuperAdministrador"))).ToListAsync();
            return users;
        }
        public async Task<User> FindUser(UserForLoginDto userForLoginDto)
        {
            var resultado = await _unitOfWork.UserRepository.FindByName(userForLoginDto.UsernameEmail);
            if (resultado == null)
            {
                resultado = await _unitOfWork.UserRepository.FindByEmail(userForLoginDto.UsernameEmail);
                if (resultado == null)
                    return null;
            }
            if (resultado == null)
                return null;
            if (!await _unitOfWork.UserRepository.IsEmailConfirmed(resultado))
                return null;
            var checkPassword = await _unitOfWork.UserRepository.CheckPassword(resultado, userForLoginDto.Password, false);
            if (checkPassword.Succeeded)
                return resultado;
            return null;
        }
        public async Task<object> GenerateJwtToken(User user, string parametroSecurity)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
            };
            var roles = await _unitOfWork.UserRepository.GetRoles(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(parametroSecurity));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var appUser = _mapper.Map<UserRolesForReturn>(user);
            return new { user = appUser, token = tokenHandler.WriteToken(token) };
        }
        public async Task<IdentityResult> CreateUser(UserForRegisterDto userforRegisterDto)
        {
            var mapped = _mapper.Map<User>(userforRegisterDto);
            var resultado = await _unitOfWork.UserRepository.CreateUser(mapped, userforRegisterDto.Password);
            return resultado;
        }
        public async Task<User> GetUsuario(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            return user;
        }
        public async Task<IdentityResult> UpdateUsuario(UserUpdateDto userForUpdateDto)
        {
            var user = await _unitOfWork.UserRepository.GetById(userForUpdateDto.Id);
            var mapped = _mapper.Map(userForUpdateDto, user);
            var resultado = await _unitOfWork.UserRepository.UpdateUsuario(mapped);
            return resultado;
        }
        public async Task<IdentityResult> UpdateEmail(UpdateEmailDto userForUpdateDto)
        {
            var user = await _unitOfWork.UserRepository.GetById(userForUpdateDto.Id);
            var token = await _unitOfWork.UserRepository.GenerateEmailChangeToken(user, userForUpdateDto.Email);
            var resultado = await _unitOfWork.UserRepository.UpdateEmail(user, userForUpdateDto.Email, token);
            return resultado;
        }
        public async Task<IdentityResult> CambiarEstado(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            if (user.Estado.Equals("Activo"))
                user.Estado = "Inactivo";
            else
                user.Estado = "Activo";
            var resultado = await _unitOfWork.UserRepository.UpdateUsuario(user);
            return resultado;
        }
        public async Task<IdentityResult> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            await _seguimientoService.DesasignarSeguimientoFromUser(user.Id);

            var resultado = await _unitOfWork.UserRepository.DeleteUser(user);
            return resultado;
        }
        public async Task<IdentityResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return null;
            var user = await _unitOfWork.UserRepository.FindById(userId);
            if (user == null)
                return null;
            return await _unitOfWork.UserRepository.ConfirmEmail(user, token);
        }
        public async Task<string> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _unitOfWork.UserRepository.FindByEmail(forgotPasswordDto.Email);
            if (user != null)
            {
                if (await _unitOfWork.UserRepository.IsEmailConfirmed(user))
                {
                    string token = await _unitOfWork.UserRepository.GeneratePasswordResetToken(user);
                    byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(token);
                    var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                    return codeEncoded;
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
        public async Task<IdentityResult> ResetPassword(int id, string password)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            string token = await _unitOfWork.UserRepository.GeneratePasswordResetToken(user);
            var resultado = await _unitOfWork.UserRepository.ResetPassword(user, token, password);
            return resultado;
        }
        public async Task<IdentityResult> ResetPasswordExterno(ResetPasswordDto resetPasswordDto)
        {
            var user = await _unitOfWork.UserRepository.FindByEmail(resetPasswordDto.Email);
            var codeDecodedBytes = WebEncoders.Base64UrlDecode(resetPasswordDto.Token);
            var codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);
            var resultado = await _unitOfWork.UserRepository.ResetPassword(user, codeDecoded, resetPasswordDto.Password);
            return resultado;
        }
    }
}