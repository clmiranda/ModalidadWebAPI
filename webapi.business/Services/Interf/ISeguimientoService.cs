using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Pagination;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface ISeguimientoService
    {
        IEnumerable<Seguimiento> GetAll();
        Task<PaginationList<Seguimiento>> GetAllSeguimiento(SeguimientoParametros parametros);
        IEnumerable<User> GetAllVoluntarios();
        Task<Seguimiento> GetById(int id);
        void CreateSeguimiento(int idAdopcion);
        Task<bool> DeleteSeguimiento(Seguimiento seguimiento);
        Task<bool> AsignarSeguimiento(int id, int idUser);
        Task<bool> QuitarAsignacion(int id, int idUser);
    }
}
