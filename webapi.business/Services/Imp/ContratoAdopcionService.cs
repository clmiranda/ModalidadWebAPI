using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class ContratoAdopcionService : IContratoAdopcionService
    {
        private IUnitOfWork _unitOfWork;
        public ContratoAdopcionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ContratoAdopcion> GetById(int id)
        {
            return await _unitOfWork.ContratoAdopcionRepository.GetById(id);
        }
        public async Task<IEnumerable<ContratoAdopcion>> GetAllAdopcionesPendientes() {
            return await _unitOfWork.ContratoAdopcionRepository.GetAllAdopcionesPendientes();
        }
        public async Task<bool> CreateContratoAdopcion(ContratoAdopcion contrato)
        {
            _unitOfWork.ContratoAdopcionRepository.Insert(contrato);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateContratoAdopcion(ContratoAdopcion contrato) {
            _unitOfWork.ContratoAdopcionRepository.Update(contrato);
            return await _unitOfWork.SaveAll();
        }
        public async void ContratoEstadoMascota(Mascota mascota)
        {
            _unitOfWork.MascotaRepository.ContratoEstadoMascota(mascota);
            await _unitOfWork.SaveAll();
        }
        public int GetLast() {
            return _unitOfWork.ContratoAdopcionRepository.GetLast().Id;
        }
        public ContratoAdopcion GetContratoByIdMascota(int id)
        {
            return _unitOfWork.ContratoAdopcionRepository.GetContratoByIdMascota(id);
        }
        public async Task<bool> AprobarAdopcion(ContratoAdopcion contrato) {
            _unitOfWork.ContratoAdopcionRepository.AprobarAdopcion(contrato);
            _unitOfWork.MascotaRepository.AprobarAdopcion(await _unitOfWork.MascotaRepository.GetById(contrato.MascotaId));
            return await _unitOfWork.SaveAll();
        }

        public async Task<bool> RechazarAdopcion(ContratoAdopcion contrato)
        {
            _unitOfWork.ContratoAdopcionRepository.RechazarAdopcion(contrato);
            _unitOfWork.MascotaRepository.RechazarAdopcion(await _unitOfWork.MascotaRepository.GetById(contrato.MascotaId));
            return await _unitOfWork.SaveAll();
        }

        public async Task<bool> CancelarAdopcion(ContratoAdopcion contrato)
        {
            _unitOfWork.ContratoAdopcionRepository.CancelarAdopcion(contrato);
            _unitOfWork.MascotaRepository.CancelarAdopcion(await _unitOfWork.MascotaRepository.GetById(contrato.MascotaId));
            return await _unitOfWork.SaveAll();
        }
    }
}