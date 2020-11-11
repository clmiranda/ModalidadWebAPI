using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface IFotoRepository: IRepository<Foto>
    {
        Task<Foto> GetPhotoPrincipalMascota(int id);
    }
}
