using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IContratoAdopcionService
    {
        Task<ContratoAdopcion> GetById(int id);
        IEnumerable<ContratoAdopcion> GetAllAdopcionesPendientes();
        Task<bool> CreateContratoAdopcion(ContratoAdopcion contrato);
        Task<bool> UpdateContratoAdopcion(ContratoAdopcion contrato);
        int GetLast();
        ContratoAdopcion GetContratoByIdMascota(int id);
        void ModifyStateMascota(int id);
        Task<bool> AprobarAdopcion(ContratoAdopcion contrato);
    }
}
