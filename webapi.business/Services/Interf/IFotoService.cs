using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
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
        Task<string> AgregarFotoMascota(int id, FotoForCreationDto fotoMascota);
        Task<bool> EliminarFoto(int id, int idfoto, string valor);
        Task<bool> AgregarFotoReporte(int id, IFormFile fotoReporte);
    }
}
