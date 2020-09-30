using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface IMascotaRepository: IRepository<Mascota>
    {
        //Task<Mascota> GetMascotaByIdCaso(int id);
        public IEnumerable<Mascota> GetAllMascotaAdopcion();
        IEnumerable<Mascota> GetAllMascotas();
        IEnumerable<Foto> GetAllFotosMascota(int id);
        Task<Foto> GetFoto(int id);
        int GetIdLastMascota();
        public Task<bool> SaveAll();
        Task<Foto> SetFotoPrincipal(int id);
        void ContratoEstadoMascota(Mascota mascota);
        void AprobarAdopcion(Mascota mascota);
        void RechazarAdopcion(Mascota mascota);
        void CancelarAdopcion(Mascota mascota);
    }
}
