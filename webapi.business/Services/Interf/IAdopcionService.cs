using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.SolicitudAdopcionCancelada;
using webapi.business.Dtos.SolicitudAdopcionRechazada;
using webapi.business.Pagination;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IAdopcionService
    {
        Task<IEnumerable<SolicitudAdopcion>> GetAllAdopcionesForReport();
        Task<PaginationList<SolicitudAdopcion>> GetAllAdopciones(AdopcionParametros parametros);
        Task<SolicitudAdopcion> GetById(int id);
        Task<SolicitudAdopcion> CreateSolicitudAdopcion(SolicitudAdopcionForCreate solicitudAdopcionDto);
        Task<bool> CreateAdopcionPresencial(AdopcionPresencialForCreateDto adopcionPresencial);
        Task<bool> UpdateFecha(FechaSolicitudAdopcionForUpdateDto fechaSolicitudAdopcionDto);
        Task<bool> CreateSolicitudAdopcionRechazada(SolicitudAdopcionRechazadaForCreateDto solicitudAdopcionRechazadaDto);
        Task<bool> CreateSolicitudAdopcionCancelada(SolicitudAdopcionCanceladaForCreateDto solicitudAdopcionCanceladaDto);
        Task<bool> AprobarSolicitudAdopcion(int id);
        Task<bool> RechazarSolicitudAdopcion(int id, int mascotaId);
        Task<bool> CancelarAdopcion(int id, int mascotaId);
        Task<bool> DeleteAllSolicitudAdopcion(int mascotaId);
    }
}
