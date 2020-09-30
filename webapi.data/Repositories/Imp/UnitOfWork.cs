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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IUserRepository _userRepository;
        private IAuthorRepository _authorRepository;
        private IRepository<Book> _bookRepository;
        private IRepository<Denuncia> _denunciaRepository;
        //private ICasoMascotaRepository _casoMascotaRepository;
        private IMascotaRepository _mascotaRepository;
        private IContratoAdopcionRepository _contratoAdopcionRepository;
        private ISeguimientoRepository _seguimientoRepository;
        private IReporteSeguimientoRepository _reporteSeguimientoRepository;
        private IRepository<ContratoRechazo> _contratoRechazoRepository;
        public UnitOfWork(BDSpatContext databaseContext,
            SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _databaseContext = databaseContext;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        public IAuthorRepository AuthorRepository
        {
            get { return _authorRepository = _authorRepository ?? new AuthorRepository(_databaseContext); }
        }

        public IRepository<Book> BookRepository
        {
            get { return _bookRepository = _bookRepository ?? new Repository<Book>(_databaseContext); }
        }

        public IUserRepository UserRepository
        {
            get { return _userRepository = _userRepository ?? new UserRepository(_databaseContext, _userManager, _signInManager); }
        }
        public IRepository<Denuncia> DenunciaRepository
        {
            get { return _denunciaRepository = _denunciaRepository ?? new Repository<Denuncia>(_databaseContext); }
        }
        //public ICasoMascotaRepository CasoMascotaRepository
        //{
        //    get { return _casoMascotaRepository = _casoMascotaRepository ?? new CasoMascotaRepository(_databaseContext); }
        //}
        public IMascotaRepository MascotaRepository
        {
            get { return _mascotaRepository = _mascotaRepository ?? new MascotaRepository(_databaseContext); }
        }
        public IContratoAdopcionRepository ContratoAdopcionRepository
        {
            get { return _contratoAdopcionRepository = _contratoAdopcionRepository ?? new ContratoAdopcionRepository(_databaseContext); }
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

        public async Task<bool> SaveAll()
        { return await _databaseContext.SaveChangesAsync() > 0; }

        public void Rollback()
        { _databaseContext.Dispose(); }
    }
}