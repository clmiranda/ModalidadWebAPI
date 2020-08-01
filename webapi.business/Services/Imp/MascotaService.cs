using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class MascotaService : IMascotaService
    {
        private IUnitOfWork _unitOfWork;
        public MascotaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CreateMascota(Mascota mascota)
        {
            mascota.FechaAgregado = DateTime.Now;
            _unitOfWork.MascotaRepository.Insert(mascota);
            return await _unitOfWork.SaveAll();
        }

        public async Task<bool> DeleteMascota(Mascota mascota)
        {
            _unitOfWork.MascotaRepository.Delete(mascota);
            return await _unitOfWork.SaveAll();
        }

        public IEnumerable<Mascota> GetAllMascotaAdopcion()
        {
            return _unitOfWork.MascotaRepository.GetAllMascotaAdopcion();
        }

        public IEnumerable<Mascota> GetAllMascotas()
        {
            return _unitOfWork.MascotaRepository.GetAllMascotas();
        }

        public int GetIdLastMascota()
        {
            return _unitOfWork.MascotaRepository.GetIdLastMascota();
        }

        public async Task<Mascota> GetMascotaById(int id)
        {
            return await _unitOfWork.MascotaRepository.GetById(id);
        }

        public Task<Mascota> GetMascotaByIdCaso(int id)
        {
            return _unitOfWork.MascotaRepository.GetMascotaByIdCaso(id);
        }

        public async Task<bool> UpdateMascota(Mascota mascota)
        {
            _unitOfWork.MascotaRepository.Update(mascota);
            return await _unitOfWork.SaveAll();
        }
    }
}
