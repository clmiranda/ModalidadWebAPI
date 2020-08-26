using System;
using System.Collections.Generic;
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
        public async Task<bool> CreateSeguimiento(Seguimiento seguimiento)
        {
             _unitOfWork.SeguimientoRepository.Insert(seguimiento);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateSeguimiento(Seguimiento seguimiento)
        {
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            for (int i = 0; i < seguimiento.CantidadVisitas; i++)
            {
                ReporteSeguimiento reporteSeguimiento = new ReporteSeguimiento
                {
                    FechaReporte = DateTime.Now,
                    Estado = "Activo",
                    SeguimientoId = seguimiento.Id
                };
                seguimiento.ReporteSeguimientos.Add(reporteSeguimiento);
            }
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
            user.Seguimientos.Add(seguimiento);
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            return await _unitOfWork.SaveAll();
        }
    }
}