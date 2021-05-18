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
        Task<bool> AgregarFotoMascota(int id, FotoForCreationDto fotoMascota);
        Task<bool> EliminarFoto(int id, int idfoto);
        Task<bool> AgregarFotoReporte(int id, IFormFile fotoReporte);
    }
}
