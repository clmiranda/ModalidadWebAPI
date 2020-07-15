using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Denuncias;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class DenunciaService : IDenunciaService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DenunciaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async  Task<IEnumerable<Denuncia>> GetAllDenuncias()
        {
            var resul= await _unitOfWork.DenunciaRepository.GetAll()/*.Include(x=>x.CasoMascotas).ToListAsync()*/;
            return resul;
        }
        public async Task<Denuncia> GetDenunciaById(int id)
        {
            var obj= await _unitOfWork.DenunciaRepository.GetById(id);
            return obj;
        }
        public async Task<bool> CreateDenuncia(Denuncia denuncia)
        {
            _unitOfWork.DenunciaRepository.Insert(denuncia);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateDenuncia(Denuncia denuncia)
        {
            _unitOfWork.DenunciaRepository.Update(denuncia);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> DeleteDenuncia(Denuncia denuncia)
        {
            _unitOfWork.DenunciaRepository.Delete(denuncia);
            return await _unitOfWork.SaveAll();
        }
    }
}
