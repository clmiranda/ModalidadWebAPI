﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface IUserRepository
    {
        IQueryable<User> FindByCondition(Expression<Func<User, bool>> expression);
        Task<IdentityResult> PostUsuario(User u, string password);
        Task<IdentityResult> UpdateUsuario(User u);
        Task<IdentityResult> DeleteUsuario(User u);
        Task<string> GenerateEmailToken(User u);
        Task<string> GeneratePasswordResetToken(User usuario);
        Task<User> FindById(string userId);
        Task<User> FindByName(string userName);
        Task<User> FindByEmail(string userEmail);
        Task<IdentityResult> ConfirmEmail(User u, string token);
        Task<bool> IsEmailConfirmed(User u);
        Task<IdentityResult> ResetPassword(User u, string token, string password);
        Task<SignInResult> CheckPassword(User u, string password, bool f);
        Task<IList<string>> GetRoles(User u);
        Task<User> GetById(int id);
        IQueryable<User> GetAll();
    }
}
