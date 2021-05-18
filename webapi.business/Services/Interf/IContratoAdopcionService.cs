using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.ContratoRechazo;
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
        Task<ContratoAdopcion> CreateContratoAdopcion(ContratoAdopcionForCreate contrato);
        Task<bool> UpdateContratoAdopcion(FechaContratoForUpdateDto dto);
        Task<bool> CreateContratoRechazo(ContratoRechazoForCreateDto contratoRechazo);
        Task<bool> AprobarAdopcion(int id, int mascotaId);
        Task<bool> RechazarAdopcion(int id, int mascotaId);
        Task<bool> CancelarAdopcion(int id, int mascotaId);
        IQueryable<ContratoAdopcion> FindByCondition(Expression<Func<ContratoAdopcion, bool>> expression);
    }
}
