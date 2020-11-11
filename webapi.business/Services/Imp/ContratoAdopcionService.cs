using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.ContratoRechazo;
using webapi.business.Helpers;
using webapi.business.Pagination;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class ContratoAdopcionService : IContratoAdopcionService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ISeguimientoService _seguimientoService;
        public ContratoAdopcionService(IUnitOfWork unitOfWork, IMapper mapper, ISeguimientoService seguimientoService)
        {
            _unitOfWork = unitOfWork;
            _seguimientoService = seguimientoService;
            _mapper = mapper;
        }
        public async Task<PaginationContratoAdopcion> GetAll(ContratoAdopcionParametros parametros)
        {
            var resul = await _unitOfWork.ContratoAdopcionRepository.GetAll();
            var x = _mapper.Map<IEnumerable<ContratoAdopcionReturnDto>>(resul).AsQueryable();
            //var lista = x.OrderByDescending(x => x.Titulo).AsQueryable();
            if (String.IsNullOrEmpty(parametros.Busqueda))
                parametros.Busqueda = "";
            if (String.IsNullOrEmpty(parametros.Filter))
                parametros.Filter = "";
            if (parametros.Filter == "Pendiente")
                x = x.Where(x => x.Estado.Equals("Pendiente") && x.Pregunta7.ToLower().Contains(parametros.Busqueda.ToLower()));
            else if (parametros.Filter == "Rechazado")
                x = x.Where(x => x.Estado.Equals("Rechazado") && x.Pregunta7.ToLower().Contains(parametros.Busqueda.ToLower()));
            else if (parametros.Filter == "Cancelado")
                x = x.Where(x => x.Estado.Equals("Cancelado") && x.Pregunta7.ToLower().Contains(parametros.Busqueda.ToLower()));
            var pagination = /*await*/ PaginationList<ContratoAdopcionReturnDto>.ToPagedList(x, parametros.PageNumber, parametros.PageSize);
            PaginationContratoAdopcion paginationReturn = new PaginationContratoAdopcion
            {
                Items = pagination,
                CurrentPage = pagination.CurrentPage,
                PageSize = pagination.PageSize,
                TotalPages = pagination.TotalPages,
                TotalCount = pagination.TotalCount
            };
            return paginationReturn;
            //return resul;
        }
        public async Task<ContratoAdopcion> GetById(int id)
        {
            return await _unitOfWork.ContratoAdopcionRepository.GetById(id);
        }
        //public async Task<IEnumerable<ContratoAdopcion>> GetAllAdopcionesPendientes() {
        //    return await _unitOfWork.ContratoAdopcionRepository.GetAllAdopcionesPendientes();
        //}
        public async Task<bool> CreateContratoAdopcion(ContratoAdopcionForCreate contrato)
        {
            //Se usa el GetById para asignar la mascota al contrato, ya que desde el MVC
            //no se esta enviando el Model Mascota del Contrato.
            var mascota = await _unitOfWork.MascotaRepository.GetById(contrato.MascotaId);
            var modelo = _mapper.Map<ContratoAdopcion>(contrato);
            modelo.Mascota = mascota;
            _unitOfWork.ContratoAdopcionRepository.Insert(modelo);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateContratoAdopcion(ContratoAdopcion contrato) {
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);
            return await _unitOfWork.SaveAll();
        }
        //public async Task<bool> ContratoEstadoMascota(Mascota mascota)
        //{
        //    _unitOfWork.MascotaRepository.ContratoEstadoMascota(mascota);
        //    return await _unitOfWork.SaveAll();
        //}
        public int GetLast() {
            return _unitOfWork.ContratoAdopcionRepository.GetLast().Id;
        }
        //public ContratoAdopcion GetContratoByIdMascota(int id)
        //{
        //    return _unitOfWork.ContratoAdopcionRepository.GetContratoByIdMascota(id);
        //}
        public async Task<bool> CreateContratoRechazo(ContratoRechazoForCreateDto contratoRechazo) {
            var contrato= await _unitOfWork.ContratoAdopcionRepository.GetById(contratoRechazo.ContratoAdopcionId);
            ContratoRechazo c = new ContratoRechazo();
            c.RazonRechazo = contratoRechazo.RazonRechazo;
            c.ContratoAdopcion = contrato;
            c.ContratoAdopcionId = contrato.Id;
            _unitOfWork.ContratoRechazoRepository.Insert(c);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> AprobarAdopcion(int id) {
            var contrato = await _unitOfWork.ContratoAdopcionRepository.GetById(id);
            contrato.Estado = "Aprobado";
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);
            var mascota = await _unitOfWork.MascotaRepository.GetById(contrato.MascotaId);
            mascota.EstadoSituacion = "Adoptada";
            _unitOfWork.MascotaRepository.Update(mascota);
            if (await _seguimientoService.CreateSeguimiento(contrato))
            {
                var seguimiento = _unitOfWork.SeguimientoRepository.FindByCondition(x=>x.Estado.Equals("Activo")).LastOrDefault();
                for (int i = 0; i < 3; i++)
                {
                    ReporteSeguimiento reporteSeguimiento = new ReporteSeguimiento();
                    //FechaReporte = DateTime.Now.Date,
                    reporteSeguimiento.Seguimiento = seguimiento;
                    reporteSeguimiento.SeguimientoId = seguimiento.Id;
                    reporteSeguimiento.Estado = "Activo";
                    seguimiento.ReporteSeguimientos.Add(reporteSeguimiento);
                }
                return await _unitOfWork.SaveAll();
            }
            else
                return false;
        }

        public async Task<bool> RechazarAdopcion(int id)
        {
            var contrato = await _unitOfWork.ContratoAdopcionRepository.GetById(id);
            contrato.Estado = "Rechazado";
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);
            var mascota = await _unitOfWork.MascotaRepository.GetById(contrato.MascotaId);
            mascota.EstadoSituacion = "Inactivo";
            mascota.ContratoAdopcion = null;
            _unitOfWork.MascotaRepository.Update(mascota);
            //_unitOfWork.SeguimientoRepository.Delete(contrato.Seguimiento);
            //_unitOfWork.ContratoAdopcionRepository.Delete(contrato);
            return await _unitOfWork.SaveAll();
        }

        public async Task<bool> CancelarAdopcion(int id)
        {
            var contrato = await _unitOfWork.ContratoAdopcionRepository.GetById(id);
            contrato.Estado = "Cancelado";
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);
            var mascota = await _unitOfWork.MascotaRepository.GetById(contrato.MascotaId);
            mascota.EstadoSituacion = "Inactivo";
            mascota.ContratoAdopcion = null;
            _unitOfWork.MascotaRepository.Update(mascota);
            _unitOfWork.SeguimientoRepository.Delete(contrato.Seguimiento);
            //_unitOfWork.ContratoAdopcionRepository.Delete(contrato);
            return await _unitOfWork.SaveAll();
        }
        public IQueryable<ContratoAdopcion> FindByCondition(Expression<Func<ContratoAdopcion, bool>> expression)
        {
            return _unitOfWork.ContratoAdopcionRepository.FindByCondition(expression).AsQueryable();
        }
    }
}