using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using webapi.business.Dtos.Fotos;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IFotoService
    {
        Task<Mascota> GetMascota(int id);
        Task<bool> SetFotoPrincipalMascota(int id, int idfoto);
        Task<string> AddFotoMascota(int idMascota, FotoForCreationDto fotoMascota);
        Task<bool> DeleteFotoMascota(int id, int idfoto);
        Task<bool> AddFotoReporte(int id, IFormFile fotoReporte);
        bool DeleteAllFotoMascota(Mascota mascota);
        Task<bool> DeleteFotoReporteSeguimiento(int idFoto);
    }
}
