using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Helpers;
using webapi.business.Pagination;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class SeguimientoService : ISeguimientoService
    {
        private IReporteSeguimientoService _reporteSeguimientoService;
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        public SeguimientoService(IUnitOfWork unitOfWork, IReporteSeguimientoService reporteSeguimientoService, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _reporteSeguimientoService = reporteSeguimientoService;
        }
        public IEnumerable<Seguimiento> GetAll() {
            var lista= _unitOfWork.SeguimientoRepository.GetAll().ToList();
            return lista;
        }

        public async Task<PaginationList<Seguimiento>> GetAllSeguimiento(SeguimientoParametros parametros)
        {
            var resul = _unitOfWork.SeguimientoRepository.GetAll();
            //var lista = x.OrderByDescending(x => x.Titulo).AsQueryable();
            //if (String.IsNullOrEmpty(parametros.Busqueda))
            //    parametros.Busqueda = "";
            //if (String.IsNullOrEmpty(parametros.Filter))
            //    parametros.Filter = "";


            if (parametros.Filter == "Activo")
                resul = resul.Where(x => x.Estado.Equals("Activo")/* && x.UserId == idUser*/);
            else if (parametros.Filter == "Pendiente")
                resul = resul.Where(x => x.Estado.Equals("Pendiente")/* && x.UserId == idUser*/);
            else if (parametros.Filter == "Asignado")
                resul = resul.Where(x => x.Estado.Equals("Asignado")/* && x.UserId== idUser*/);
            //var x = _mapper.Map<IEnumerable<ContratoAdopcionReturnDto>>(resul).AsQueryable();
            var pagination = await PaginationList<Seguimiento>.ToPagedList(resul, parametros.PageNumber, parametros.PageSize);
            //PaginationContratoAdopcion paginationReturn = new PaginationContratoAdopcion
            //{
            //    Items = pagination,
            //    CurrentPage = pagination.CurrentPage,
            //    PageSize = pagination.PageSize,
            //    TotalPages = pagination.TotalPages,
            //    TotalCount = pagination.TotalCount
            //};
            return pagination;
            //return resul;
        }
        public async Task<Seguimiento> GetById(int id)
        {
            return await _unitOfWork.SeguimientoRepository.GetById(id);
        }
        public async Task<Seguimiento> GetByIdContrato(int id)
        {
            return await _unitOfWork.SeguimientoRepository.GetByIdContrato(id);
        }
        public void CreateSeguimiento(ContratoAdopcion contrato)
        {
            Seguimiento seguimiento = new Seguimiento();
            seguimiento.ContratoAdopcion = contrato;
            seguimiento.ContratoAdopcionId = contrato.Id;
            seguimiento.FechaInicio = DateTime.Now;
            seguimiento.FechaConclusion = DateTime.Now;
            seguimiento.Estado = "Activo";
            _unitOfWork.SeguimientoRepository.Insert(seguimiento);
            //var obj = _unitOfWork.SeguimientoRepository.FindByCondition(x=>x.Estado.Equals("Activo")).LastOrDefault();
        }
        public async Task<bool> UpdateSeguimiento(Seguimiento seguimiento)
        {
            var objeto = await _unitOfWork.SeguimientoRepository.GetById(seguimiento.Id);
            //objeto.CantidadVisitas = seguimiento.CantidadVisitas;
            _unitOfWork.SeguimientoRepository.Update(objeto);
            //for (int i = 0; i < seguimiento.CantidadVisitas; i++)
            //{
            //    ReporteSeguimiento reporteSeguimiento = new ReporteSeguimiento();
            //    //FechaReporte = DateTime.Now.Date,
            //    reporteSeguimiento.Seguimiento = objeto;
            //    reporteSeguimiento.SeguimientoId = seguimiento.Id;
            //    reporteSeguimiento.Estado = "Activo";
            //    seguimiento.ReporteSeguimientos.Add(reporteSeguimiento);
            //}
            return await _unitOfWork.SaveAll();
        }
        public IEnumerable<User> GetAllVoluntarios() {
            var resul = _unitOfWork.UserRepository.FindByCondition(x => x.UserRoles.Any(y => y.Role.Name.Equals("Voluntario"))).ToList();
            return resul;
        }
        public async Task<Seguimiento> UpdateFecha(FechaReporteDto dto) {
            var objeto = await _unitOfWork.SeguimientoRepository.GetById(dto.Id);
            //objeto.FechaInicio = dto.FechaInicio;
            //objeto.FechaConclusion = dto.FechaConclusion;
            objeto.FechaInicio = Convert.ToDateTime(dto.RangoFechas[0]);
            objeto.FechaConclusion = Convert.ToDateTime(dto.RangoFechas[1]);
            //var model = _mapper.Map<Seguimiento>(dto);
            _unitOfWork.SeguimientoRepository.Update(objeto);
            if (await _unitOfWork.SaveAll())
                return objeto;
            return null;
        }

        public async Task<bool> DeleteSeguimiento(Seguimiento seguimiento)
        {
            _unitOfWork.SeguimientoRepository.Delete(seguimiento);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> CheckedVoluntarioAsignado(int id, int idUser) {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            var user = await _unitOfWork.UserRepository.GetById(idUser);
            //seguimiento.User = user;
            seguimiento.UserId = idUser;
            seguimiento.Estado = "Pendiente";
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            user.Seguimientos.Add(seguimiento);
            //_unitOfWork.UserRepository.Update(user);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> RemoveVoluntarioChecked(int id, int idUser)
        {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            var user = await _unitOfWork.UserRepository.GetById(idUser);
            seguimiento.User = null;
            seguimiento.Estado = "Activo";
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            user.Seguimientos.Remove(seguimiento);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> AsignarSeguimiento(int id)
        {
            var resul = await _unitOfWork.SeguimientoRepository.GetById(id);
            resul.Estado = "Asignado";
            _unitOfWork.SeguimientoRepository.Update(resul);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> RechazarSeguimiento(int id)
        {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            seguimiento.User = null;
            //resul.UserId = 0;
            seguimiento.Estado = "Activo";
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            return await _unitOfWork.SaveAll();
        }
        public IEnumerable<Seguimiento> GetSeguimientoForVoluntario(int userId)
        {
            var resul = _unitOfWork.SeguimientoRepository.FindByCondition(x=>x.UserId== userId).ToList();
            return resul;
        }
    }
}