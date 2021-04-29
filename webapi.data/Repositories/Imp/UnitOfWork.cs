using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.data.Repositories.Imp
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly BDSpatContext _databaseContext;
        private readonly UserManager<User> _userManejador;
        private readonly SignInManager<User> _signInManejador;
        private IUserRepository _userRepository;
        private IRepository<Denuncia> _denunciaRepository;
        //private ICasoMascotaRepository _casoMascotaRepository;
        private IRepository<Mascota> _mascotaRepository;
        private IRepository<Foto> _fotoRepository;
        private IRepository<ContratoAdopcion> _contratoAdopcionRepository;
        private ISeguimientoRepository _seguimientoRepository;
        private IReporteSeguimientoRepository _reporteSeguimientoRepository;
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
            get { return _userRepository = _userRepository ?? new UserRepository(_databaseContext, _userManejador, _signInManejador); }
        }
        public IRepository<Denuncia> DenunciaRepository
        {
            get { return _denunciaRepository = _denunciaRepository ?? new Repository<Denuncia>(_databaseContext); }
        }
        //public ICasoMascotaRepository CasoMascotaRepository
        //{
        //    get { return _casoMascotaRepository = _casoMascotaRepository ?? new CasoMascotaRepository(_databaseContext); }
        //}
        public IRepository<Mascota> MascotaRepository
        {
            get { return _mascotaRepository = _mascotaRepository ?? new Repository<Mascota>(_databaseContext); }
        }
        public IRepository<Foto> FotoRepository
        {
            get { return _fotoRepository = _fotoRepository ?? new Repository<Foto>(_databaseContext); }
        }
        public IRepository<ContratoAdopcion> ContratoAdopcionRepository
        {
            get { return _contratoAdopcionRepository = _contratoAdopcionRepository ?? new Repository<ContratoAdopcion>(_databaseContext); }
        }
        public ISeguimientoRepository SeguimientoRepository
        {
            get { return _seguimientoRepository = _seguimientoRepository ?? new SeguimientoRepository(_databaseContext); }
        }
        public IReporteSeguimientoRepository ReporteSeguimientoRepository
        {
            get { return _reporteSeguimientoRepository = _reporteSeguimientoRepository ?? new ReporteSeguimientoRepository(_databaseContext); }
        }

        public IRepository<ContratoRechazo> ContratoRechazoRepository
        {
            get { return _contratoRechazoRepository = _contratoRechazoRepository ?? new Repository<ContratoRechazo>(_databaseContext); }
        }

        public IRolUserRepository RolUserRepository
        {
            get { return _rolUserRepository = _rolUserRepository ?? new RolUserRepository(_userManejador); }
        }

        public IRepository<ReporteTratamiento> ReporteTratamientoRepository
        {
            get { return _reporteTratamientoRepository = _reporteTratamientoRepository ?? new Repository<ReporteTratamiento>(_databaseContext); }
        }

        public async Task<bool> SaveAll()
        { return await _databaseContext.SaveChangesAsync() > 0; }

        public void Rollback()
        { _databaseContext.Dispose(); }
    }
}