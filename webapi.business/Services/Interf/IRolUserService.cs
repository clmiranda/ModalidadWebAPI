using System.Collections.Generic;
using System.Threading.Tasks;

namespace webapi.business.Services.Interf
{
    public interface IRolUserService
    {
        Task<IEnumerable<string>> AsignarRoles(int id, string[] rolesUser);
    }
}
