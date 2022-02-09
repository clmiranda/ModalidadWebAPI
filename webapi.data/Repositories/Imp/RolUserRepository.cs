using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class RolUserRepository : IRolUserRepository
    {
        private readonly UserManager<User> _userManejador;
        public RolUserRepository(UserManager<User> userManejador)
        {
            _userManejador = userManejador;
        }
        public async Task<IdentityResult> AsignarRolesUser(User user, IEnumerable<string> roles)
        {
            return await _userManejador.AddToRolesAsync(user, roles);
        }

        public async Task<IList<string>> GetRolesUser(User user)
        {
            return await _userManejador.GetRolesAsync(user);
        }

        public async Task<IdentityResult> QuitarRolesUser(User user, IEnumerable<string> roles)
        {
            return await _userManejador.RemoveFromRolesAsync(user, roles);
        }
    }
}
