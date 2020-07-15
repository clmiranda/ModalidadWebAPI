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
        private IUnitOfWork _unitOfWork;
        public SeguimientoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Seguimiento> GetById(int id)
        {
            return await _unitOfWork.SeguimientoRepository.GetById(id);
        }
        public async Task<bool> CreateSeguimiento(Seguimiento seguimiento)
        {
             _unitOfWork.SeguimientoRepository.Insert(seguimiento);
            return await _unitOfWork.SaveAll();
        }
    }
}