using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BDSpatContext _databaseContext;
        private readonly UserManager<User> _userManejador;
        private readonly SignInManager<User> _signInManejador;
        private IUserRepository _userRepository;
        private IRepository<Denuncia> _denunciaRepository;
        private IRepository<Mascota> _mascotaRepository;
        private IRepository<Foto> _fotoRepository;
        private IRepository<ContratoAdopcion> _contratoAdopcionRepository;
        private IRepository<Seguimiento> _seguimientoRepository;
        private IRepository<ReporteSeguimiento> _reporteSeguimientoRepository;
        private IRepository<ContratoRechazo> _contratoRechazoRepository;
        private IRepository<ReporteTratamiento> _reporteTratamientoRepository;
        private IRolUserRepository _rolUserRepository;
        public UnitOfWork(BDSpatContext databaseContext,
            SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _databaseContext = databaseContext;
            _signInManejador = signInManager;
            _userManejador = userManager;

        }

        public IUserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_databaseContext, _userManejador, _signInManejador); }
        }
        public IRepository<Denuncia> DenunciaRepository
        {
            get { return _denunciaRepository ??= new Repository<Denuncia>(_databaseContext); }
        }
        public IRepository<Mascota> MascotaRepository
        {
            get { return _mascotaRepository ??= new Repository<Mascota>(_databaseContext); }
        }
        public IRepository<Foto> FotoRepository
        {
            get { return _fotoRepository ??= new Repository<Foto>(_databaseContext); }
        }
        public IRepository<ContratoAdopcion> ContratoAdopcionRepository
        {
            get { return _contratoAdopcionRepository ??= new Repository<ContratoAdopcion>(_databaseContext); }
        }
        public IRepository<Seguimiento> SeguimientoRepository
        {
            get { return _seguimientoRepository ??= new Repository<Seguimiento>(_databaseContext); }
        }
        public IRepository<ReporteSeguimiento> ReporteSeguimientoRepository
        {
            get { return _reporteSeguimientoRepository ??= new Repository<ReporteSeguimiento>(_databaseContext); }
        }

        public IRepository<ContratoRechazo> ContratoRechazoRepository
        {
            get { return _contratoRechazoRepository ??= new Repository<ContratoRechazo>(_databaseContext); }
        }

        public IRolUserRepository RolUserRepository
        {
            get { return _rolUserRepository ??= new RolUserRepository(_userManejador); }
        }

        public IRepository<ReporteTratamiento> ReporteTratamientoRepository
        {
            get { return _reporteTratamientoRepository ??= new Repository<ReporteTratamiento>(_databaseContext); }
        }

        public async Task<bool> SaveAll()
        { return await _databaseContext.SaveChangesAsync() > 0; }

        public void Rollback()
        { _databaseContext.Dispose(); }
    }
}