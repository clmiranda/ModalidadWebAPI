using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class SeguimientoService : ISeguimientoService
    {
        private IReporteSeguimientoService _reporteSeguimientoService;
        private IUnitOfWork _unitOfWork;
        public SeguimientoService(IUnitOfWork unitOfWork, IReporteSeguimientoService reporteSeguimientoService)
        {
            _unitOfWork = unitOfWork;
            _reporteSeguimientoService = reporteSeguimientoService;
        }
        public async Task<IEnumerable<Seguimiento>> GetAll() {
            var lista= await _unitOfWork.SeguimientoRepository.GetAll();
            return lista;
        }
        public async Task<Seguimiento> GetById(int id)
        {
            return await _unitOfWork.SeguimientoRepository.GetById(id);
        }
        public async Task<Seguimiento> GetByIdContrato(int id)
        {
            return await _unitOfWork.SeguimientoRepository.GetByIdContrato(id);
        }
        public async Task<bool> CreateSeguimiento(ContratoAdopcion contrato)
        {
            Seguimiento seguimiento = new Seguimiento();
            seguimiento.ContratoAdopcion = contrato;
            seguimiento.ContratoAdopcionId = contrato.Id;
            seguimiento.Estado = "Activo";
            _unitOfWork.SeguimientoRepository.Insert(seguimiento);
            //var obj = _unitOfWork.SeguimientoRepository.FindByCondition(x=>x.Estado.Equals("Activo")).LastOrDefault();
            return await _unitOfWork.SaveAll();
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

        public async Task<bool> DeleteSeguimiento(Seguimiento seguimiento)
        {
            _unitOfWork.SeguimientoRepository.Delete(seguimiento);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> CheckedVoluntarioAsignado(Seguimiento seguimiento, User user) {
            seguimiento.User = user;
            seguimiento.UserId = user.Id;
            seguimiento.Estado = "Pendiente";
            user.Seguimientos.Add(seguimiento);
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
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
            var resul = await _unitOfWork.SeguimientoRepository.GetById(id);
            resul.User = null;
            resul.UserId = null;
            resul.Estado = "Activo";
            _unitOfWork.SeguimientoRepository.Update(resul);
            return await _unitOfWork.SaveAll();
        }
        public IEnumerable<Seguimiento> GetSeguimientoForVoluntario(int userId)
        {
            var resul = _unitOfWork.SeguimientoRepository.FindByCondition(x=>x.UserId== userId).ToList();
            return resul;
        }
    }
}