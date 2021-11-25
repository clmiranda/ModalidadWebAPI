using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using webapi.business.Dtos.Fotos;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IFotoService
    {
        Task<Foto> GetFoto(int id);
        Task<Mascota> GetMascota(int id);
        Task<bool> SetFotoPrincipalMascota(int id, int idfoto);
        Task<bool> AddFotoMascota(int id, FotoForCreationDto fotoMascota);
        Task<bool> DeleteFotoMascota(int id, int idfoto);
        Task<bool> AgregarFotoReporte(int id, IFormFile fotoReporte);
    }
}
