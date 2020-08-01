using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IMascotaService
    {
        IEnumerable<Mascota> GetAllMascotas();
        IEnumerable<Mascota> GetAllMascotaAdopcion();
        Task<Mascota> GetMascotaById(int id);
        int GetIdLastMascota();
        Task<Mascota> GetMascotaByIdCaso(int id);
        Task<bool> CreateMascota(Mascota mascota);
        Task<bool> UpdateMascota(Mascota mascota);
        Task<bool> DeleteMascota(Mascota mascota);
    }
}
