using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.ContratoRechazo;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IContratoAdopcionService
    {
        Task<ContratoAdopcion> GetById(int id);
        Task<IEnumerable<ContratoAdopcion>> GetAllAdopcionesPendientes();
        Task<bool> CreateContratoAdopcion(ContratoAdopcion contrato);
        Task<bool> UpdateContratoAdopcion(ContratoAdopcion contrato);
        int GetLast();
        ContratoAdopcion GetContratoByIdMascota(int id);
        Task<bool> ContratoEstadoMascota(Mascota mascota);
        Task<bool> CreateContratoRechazo(ContratoRechazoForCreateDto contratoRechazo);
        Task<bool> AprobarAdopcion(int id);
        Task<bool> RechazarAdopcion(int id);
        Task<bool> CancelarAdopcion(int id);
    }
}
