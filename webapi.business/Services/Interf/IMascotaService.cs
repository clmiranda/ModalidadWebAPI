using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using webapi.business.Dtos.Mascotas;
using webapi.business.Pagination;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IMascotaService
    {
        IQueryable<Mascota> FindByCondition(Expression<Func<Mascota, bool>> expression);
        IEnumerable<MascotaForDetailedDto> GetAll();
        Task<PaginationList<Mascota>> GetAllMascotas(MascotaParametros parametros);
        Task<Mascota> GetMascotaById(int id);
        Task<Mascota> CreateMascota(MascotaForCreateDto mascota);
        Task<Mascota> UpdateMascota(MascotaForUpdateDto mascota);
        Task<bool> ChangeEstado(string estado, int id);
    }
}
