using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Denuncias;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IDenunciaService
    {
         Task<IEnumerable<Denuncia>> GetAllDenuncias();
        Task<Denuncia> GetDenunciaById(int id);
        Task<bool> CreateDenuncia(Denuncia denuncia);
        Task<bool> UpdateDenuncia(Denuncia denuncia);
        Task<bool> DeleteDenuncia(Denuncia denuncia);
    }
}
