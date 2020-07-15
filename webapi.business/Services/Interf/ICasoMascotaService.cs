using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface ICasoMascotaService
    {
        Task<IEnumerable<CasoMascota>> GetAllCasosMascota();
        Task<CasoMascota> GetCasoMascotaById(int id);
        int GetIdLastCasoMascota();
        Task<CasoMascota> GetCasoMascotaByIdDenuncia(int id);
        Task<bool> CreateCasoMascota(CasoMascota casoMascota);
        Task<bool> UpdateCasoMascota(CasoMascota casoMascota);
        Task<bool> DeleteCasoMascota(CasoMascota casoMascota);
    }
}
