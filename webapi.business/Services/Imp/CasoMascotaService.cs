using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class CasoMascotaService : ICasoMascotaService
    {
        private IUnitOfWork _unitOfWork;
        public CasoMascotaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<IEnumerable<CasoMascota>> GetAllCasosMascota()
        {
            return _unitOfWork.CasoMascotaRepository.GetAll();
        }
        public Task<CasoMascota> GetCasoMascotaByIdDenuncia(int id)
        {
            return _unitOfWork.CasoMascotaRepository.GetCasoMascotaByIdDenuncia(id);
        }
        public Task<CasoMascota> GetCasoMascotaById(int id)
        {
            return _unitOfWork.CasoMascotaRepository.GetById(id);
        }
        public int GetIdLastCasoMascota() {
            return _unitOfWork.CasoMascotaRepository.GetIdLastCasoMascota();
        }
        public async Task<bool> CreateCasoMascota(CasoMascota casoMascota)
        {
            _unitOfWork.CasoMascotaRepository.Insert(casoMascota);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateCasoMascota(CasoMascota casoMascota)
        {
            _unitOfWork.CasoMascotaRepository.Update(casoMascota);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> DeleteCasoMascota(CasoMascota casoMascota)
        {
            _unitOfWork.CasoMascotaRepository.Delete(casoMascota);
            return await _unitOfWork.SaveAll();
        }
    }
}
