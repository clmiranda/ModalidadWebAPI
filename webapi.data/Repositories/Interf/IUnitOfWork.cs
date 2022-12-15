using System.Threading.Tasks;
using webapi.core.Models;

namespace webapi.data.Repositories.Interf
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IRepository<Denuncia> DenunciaRepository { get; }
        IRepository<Mascota> MascotaRepository { get; }
        IRepository<Foto> FotoRepository { get; }
        IRepository<ReporteTratamiento> ReporteTratamientoRepository { get; }
        IRepository<SolicitudAdopcion> SolicitudAdopcionRepository { get; }
        IRepository<Seguimiento> SeguimientoRepository { get; }
        IRepository<ReporteSeguimiento> ReporteSeguimientoRepository { get; }
        IRepository<AdopcionRechazada> AdopcionRechazadaRepository { get; }
        IRepository<AdopcionCancelada> AdopcionCanceladaRepository { get; }
        IRepository<ContratoAdopcion> ContratoAdopcionRepository { get; }
        IRolUserRepository RolUserRepository { get; }
        Task<bool> SaveAll();
        void Rollback();
    }
}
