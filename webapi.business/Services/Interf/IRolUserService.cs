using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Usuario;

namespace webapi.business.Services.Interf
{
    public interface IRolUserService
    {
        Task<IEnumerable<string>> AsignarRoles(int id, string[] rolesUser);
    }
}
