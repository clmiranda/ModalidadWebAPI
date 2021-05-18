using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.business.Dtos.Denuncias;
using webapi.business.Pagination;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IDenunciaService
    {
        Task<IEnumerable<DenunciaForListDto>> GetAll();
        Task<PaginationList<Denuncia>> GetAllDenuncias(DenunciaParametros parametros);
        Task<Denuncia> GetDenunciaById(int id);
        Task<Denuncia> CreateDenuncia(Denuncia denuncia);
        Task<Denuncia> UpdateDenuncia(Denuncia denuncia);
        Task<bool> DeleteDenuncia(Denuncia denuncia);
    }
}
