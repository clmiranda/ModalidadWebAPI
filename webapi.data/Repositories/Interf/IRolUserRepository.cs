using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface IRolUserRepository
    {
        Task<IdentityResult> AsignarRolesUser(User user, IEnumerable<string> roles);
        Task<IdentityResult> QuitarRolesUser(User user, IEnumerable<string> roles);
        Task<IList<string>> GetRolesUser(User user);
    }
}
