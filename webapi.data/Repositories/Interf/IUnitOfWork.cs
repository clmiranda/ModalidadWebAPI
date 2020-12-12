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
        IRepository<Mascota> MascotaRepository { get; }
        IRepository<ContratoAdopcion> ContratoAdopcionRepository { get; }
        ISeguimientoRepository SeguimientoRepository { get; }
        IReporteSeguimientoRepository ReporteSeguimientoRepository { get; }
        IRepository<ContratoRechazo> ContratoRechazoRepository { get; }
        IFotoRepository FotoRepository { get; }
        IRolUserRepository RolUserRepository { get; }
        Task<bool> SaveAll();
        void Rollback();
    }
}
