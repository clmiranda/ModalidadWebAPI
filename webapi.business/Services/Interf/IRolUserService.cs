using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Usuario;

namespace webapi.business.Services.Interf
{
    public interface IRolUserService
    {
        Task<IEnumerable<string>> PutRolesUser(string nombreUsuario, string[] rolesUserDto);
    }
}
