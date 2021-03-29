using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.ContratoRechazo;
using webapi.business.Helpers;
using webapi.business.Pagination;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IContratoAdopcionService
    {
        IEnumerable<ContratoAdopcion> GetAll();
        IEnumerable<ContratoRechazo> GetAllRechazoCancelado();
        Task<PaginationList<ContratoAdopcion>> GetAllContratos(ContratoAdopcionParametros parametros);
        Task<ContratoAdopcion> GetById(int id);
        //Task<IEnumerable<ContratoAdopcion>> GetAllAdopcionesPendientes();
        Task<ContratoAdopcion> CreateContratoAdopcion(ContratoAdopcionForCreate contrato);
        Task<bool> UpdateContratoAdopcion(ContratoAdopcion contrato);
        int GetLast();
        //ContratoAdopcion GetContratoByIdMascota(int id);
        //Task<bool> ContratoEstadoMascota(Mascota mascota);
        Task<bool> CreateContratoRechazo(ContratoRechazoForCreateDto contratoRechazo);
        Task<bool> AprobarAdopcion(int id);
        Task<bool> RechazarAdopcion(int id);
        Task<bool> CancelarAdopcion(int id);
        IQueryable<ContratoAdopcion> FindByCondition(Expression<Func<ContratoAdopcion, bool>> expression);
    }
}
