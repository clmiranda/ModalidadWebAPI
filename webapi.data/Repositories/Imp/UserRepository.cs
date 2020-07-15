using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class UserRepository: IUserRepository
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

        public async Task<IdentityResult> CreateUser(User u, string password)
        {
            return await _userManager.CreateAsync(u, password);
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

        public async Task<string> GeneratePasswordResetToken(User u)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(u);
        }

        public async Task<IList<string>> GetRoles(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }
        public void AddRole(User user)
        {
             _userManager.AddToRoleAsync(user,"Adoptante").Wait();
        }

        public async Task<bool> IsEmailConfirmed(User u)
        {
            return await _userManager.IsEmailConfirmedAsync(u);
        }

        public async Task<IdentityResult> ResetPassword(User u, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(u, token, password);
        }

        //crud
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public void Insert(User u)
        {
            if (u == null) throw new ArgumentNullException("entity");
            _context.Users.Add(u);
        }

        public void Update(User u)
        {
            if (u == null) throw new ArgumentNullException("entity");
        }

        public void Delete(int id)
        {
            var u = _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (u != null)
                _context.Remove(u);
            throw new ArgumentNullException("Id no encontrado.");
        }
        public async Task<bool> SaveAll()
        { return await _context.SaveChangesAsync() > 0; }

        public void Rollback()
        { _context.Dispose(); }

        public async Task<Foto> GetFoto(int id)
        {
            var photo = await _context.Foto.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }
        public void DeleteFoto(int id)
        {
            var u = _context.Foto.FirstOrDefaultAsync(x => x.Id == id);
            if (u != null)
                _context.Remove(u);
            throw new ArgumentNullException("Id no encontrado.");
        }
    }
}
