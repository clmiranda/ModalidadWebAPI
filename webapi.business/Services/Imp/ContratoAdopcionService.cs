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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ISeguimientoService _seguimientoService;
        public ContratoAdopcionService(IUnitOfWork unitOfWork, IMapper mapper, ISeguimientoService seguimientoService)
        {
            _unitOfWork = unitOfWork;
            _seguimientoService = seguimientoService;
            _mapper = mapper;
        }
        public IEnumerable<ContratoAdopcion> GetAll()
        {
            return _unitOfWork.ContratoAdopcionRepository.GetAll().ToList();
        }
        public IEnumerable<ContratoRechazo> GetAllRechazoCancelado()
        {
            return _unitOfWork.ContratoRechazoRepository.GetAll().ToList();
        }
        public async Task<ContratoAdopcion> GetById(int id)
        {
            return await _unitOfWork.ContratoAdopcionRepository.GetById(id);
        }
        public async Task<PaginationList<ContratoAdopcion>> GetAllContratos(ContratoAdopcionParametros parametros)
        {
            var resul = _unitOfWork.ContratoAdopcionRepository.GetAll();

            if (parametros.Filter == "Aprobado")
                resul = resul.Where(x => x.Estado.Equals("Aprobado"));
            else if (parametros.Filter == "Pendiente")
                resul = resul.Where(x => x.Estado.Equals("Pendiente"));
            else if (parametros.Filter == "Rechazado")
                resul = resul.Where(x => x.Estado.Equals("Rechazado"));
            else if (parametros.Filter == "Cancelado")
                resul = resul.Where(x => x.Estado.Equals("Cancelado"));

            var pagination = await PaginationList<ContratoAdopcion>.ToPagedList(resul, parametros.PageNumber, parametros.PageSize);
            return pagination;
        }
        public async Task<ContratoAdopcion> CreateContratoAdopcion(ContratoAdopcionForCreate dto)
        {
            var mascota = await _unitOfWork.MascotaRepository.GetById(dto.MascotaId);
            //mascota.EstadoSituacion = "En Proceso";
            //_unitOfWork.MascotaRepository.Update(mascota);

            var modelo = _mapper.Map<ContratoAdopcion>(dto);
            //modelo.Estado = "Pendiente";
            modelo.Mascota = mascota;
            modelo.Mascota.Estado= "En Proceso";
            _unitOfWork.ContratoAdopcionRepository.Insert(modelo);

            if (await _unitOfWork.SaveAll())
                return modelo;
            return null;
        }

        //here
        public async Task<bool> UpdateContratoAdopcion(FechaContratoForUpdateDto dto) {
            var modelo = await _unitOfWork.ContratoAdopcionRepository.GetById(dto.Id);
            var mapped = _mapper.Map(dto, modelo);
            _unitOfWork.ContratoAdopcionRepository.Update(mapped);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> CreateContratoRechazo(ContratoRechazoForCreateDto contratoRechazo) {
            var c = _mapper.Map<ContratoRechazo>(contratoRechazo);
            _unitOfWork.ContratoRechazoRepository.Insert(c);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> AprobarAdopcion(int id, int mascotaId) {
            var mascota = await _unitOfWork.MascotaRepository.GetById(mascotaId);
            //mascota.EstadoSituacion = "Adoptada";
            //_unitOfWork.MascotaRepository.Update(mascota);

            var contrato = await _unitOfWork.ContratoAdopcionRepository.GetById(id);
            contrato.Estado = "Aprobado";
            contrato.Mascota.Estado = "Adoptada";
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);

            //Create Seguimiento
            _seguimientoService.CreateSeguimiento(contrato);
            return await _unitOfWork.SaveAll();
            //var seguimiento = _unitOfWork.SeguimientoRepository.FindByCondition(x=>x.Estado.Equals("Activo")).LastOrDefault();
            //for (int i = 0; i < 3; i++)
            //{
            //    ReporteSeguimiento reporteSeguimiento = new ReporteSeguimiento();
            //    //FechaReporte = DateTime.Now.Date,
            //    reporteSeguimiento.Seguimiento = seguimiento;
            //    reporteSeguimiento.SeguimientoId = seguimiento.Id;
            //    reporteSeguimiento.Estado = "Activo";
            //    seguimiento.ReporteSeguimientos.Add(reporteSeguimiento);
            //}
            //return await _unitOfWork.SaveAll();
        }

        public async Task<bool> RechazarAdopcion(int id, int mascotaId)
        {
            var contrato = await _unitOfWork.ContratoAdopcionRepository.GetById(id);
            contrato.Mascota = null;
            contrato.Estado = "Rechazado";
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);

            var mascota = await _unitOfWork.MascotaRepository.GetById(mascotaId);
            mascota.Estado = "Inactivo";
            //mascota.ContratoAdopcion = null;
            _unitOfWork.MascotaRepository.Update(mascota);
            //_unitOfWork.SeguimientoRepository.Delete(contrato.Seguimiento);
            //_unitOfWork.ContratoAdopcionRepository.Delete(contrato);
            return await _unitOfWork.SaveAll();
        }

        public async Task<bool> CancelarAdopcion(int id, int mascotaId)
        {
            var contrato = await _unitOfWork.ContratoAdopcionRepository.GetById(id);
            contrato.Mascota = null;
            contrato.Estado = "Cancelado";
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);

            var mascota = await _unitOfWork.MascotaRepository.GetById(mascotaId);
            mascota.Estado = "Inactivo";
            _unitOfWork.MascotaRepository.Update(mascota);
            //_unitOfWork.ContratoAdopcionRepository.Delete(contrato);
            return await _unitOfWork.SaveAll();
        }
        public IQueryable<ContratoAdopcion> FindByCondition(Expression<Func<ContratoAdopcion, bool>> expression)
        {
            return _unitOfWork.ContratoAdopcionRepository.FindByCondition(expression).AsQueryable();
        }
    }
}