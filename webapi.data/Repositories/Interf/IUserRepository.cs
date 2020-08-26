using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllVoluntarios();
        Task<IdentityResult> CreateUser(User u, string password);
        Task<string> GenerateEmailToken(User u);
        Task<string> GeneratePasswordResetToken(User u);
        Task<User> FindById(string userId);
        Task<User> FindByName(string userName);
        Task<User> FindByEmail(string userEmail);
        Task<IdentityResult> ConfirmEmail(User u, string token);
        Task<bool> IsEmailConfirmed(User u);
        Task<IdentityResult> ResetPassword(User u, string token, string password);
        Task<SignInResult> CheckPassword(User u, string password, bool f);
        Task<IList<string>> GetRoles(User u);
        Task<Foto> GetFoto(int id);
        void DeleteFoto(int id);
        void AddRole(User user);


        //crud
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        void Insert(User u);
        void Update(User u);
        void Delete(int id);
        public Task<bool> SaveAll();
        public void Rollback();
    }
}
