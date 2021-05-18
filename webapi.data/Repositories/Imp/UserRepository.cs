using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class UserRepository : IUserRepository
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly BDSpatContext _context;
        public UserRepository(BDSpatContext context,
                UserManager<User> userManager,
                SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<SignInResult> CheckPassword(User u, string password, bool f = false)
        {
            return await _signInManager.CheckPasswordSignInAsync(u, password, f);
        }
        public async Task<IdentityResult> ConfirmEmail(User u, string token)
        {
            return await _userManager.ConfirmEmailAsync(u, token);
        }
        public async Task<IdentityResult> PostUsuario(User u, string password)
        {
            return await _userManager.CreateAsync(u, password);
        }
        public async Task<IdentityResult> UpdateUsuario(User u)
        {
            return await _userManager.UpdateAsync(u);
        }
        public async Task<User> FindByEmail(string userEmail)
        {
            return await _userManager.FindByEmailAsync(userEmail);
        }
        public async Task<User> FindById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
        public async Task<User> FindByName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }
        public async Task<string> GenerateEmailToken(User u)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(u);
        }
        public async Task<string> GeneratePasswordResetToken(User usuario)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(usuario);
        }
        public async Task<IList<string>> GetRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<bool> IsEmailConfirmed(User u)
        {
            return await _userManager.IsEmailConfirmedAsync(u);
        }
        public async Task<IdentityResult> ResetPassword(User u, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(u, token, password);
        }
        public IQueryable<User> GetAll()
        {
            return _context.Users.AsQueryable();
        }
        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IdentityResult> DeleteUsuario(User u)
        {
            return await _userManager.DeleteAsync(u);
        }
        public IQueryable<User> FindByCondition(Expression<Func<User, bool>> expression)
        {
            return _context.Users.Where(expression).AsQueryable();
        }
    }
}
