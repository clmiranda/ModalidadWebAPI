using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface IUnitOfWork
    {
        IAuthorRepository AuthorRepository { get; }
        IRepository<Book> BookRepository { get; }
        IUserRepository UserRepository { get; }
        IRepository<Denuncia> DenunciaRepository { get; }
        IMascotaRepository MascotaRepository { get; }
        IContratoAdopcionRepository ContratoAdopcionRepository { get; }
        ISeguimientoRepository SeguimientoRepository { get; }
        IReporteSeguimientoRepository ReporteSeguimientoRepository { get; }
        IRepository<ContratoRechazo> ContratoRechazoRepository { get; }
        Task<bool> SaveAll();
        void Rollback();
    }
}
