using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Dtos.Denuncias;
using webapi.business.Pagination;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class DenunciaService : IDenunciaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DenunciaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DenunciaForListDto>> GetAll() {
            var lista = await _unitOfWork.DenunciaRepository.GetAll().ToListAsync();
            var mapped = _mapper.Map<IEnumerable<DenunciaForListDto>>(lista);
            return mapped;
        }
        public async  Task<PaginationList<Denuncia>> GetAllDenuncias(DenunciaParametros parametros)
        {
            var resul= _unitOfWork.DenunciaRepository.GetAll();
            if (!String.IsNullOrEmpty(parametros.Busqueda))
                resul = resul.Where(x => x.Titulo.ToLower().Contains(parametros.Busqueda.ToLower()) || x.Descripcion.ToLower().Contains(parametros.Busqueda.ToLower()));
            var pagination= await PaginationList<Denuncia>.ToPagedList(resul, parametros.PageNumber, parametros.PageSize);
            return pagination;
        }
        public async Task<Denuncia> GetDenunciaById(int id)
        {
            var denuncia= await _unitOfWork.DenunciaRepository.GetById(id);
            return denuncia;
        }
        public async Task<Denuncia> CreateDenuncia(Denuncia denuncia)
        {
            denuncia.Estado = "Activo";
            _unitOfWork.DenunciaRepository.Insert(denuncia);
            if (await _unitOfWork.SaveAll())
                return denuncia;
            return null;
        }
        public async Task<Denuncia> UpdateDenuncia(Denuncia denuncia)
        {
            _unitOfWork.DenunciaRepository.Update(denuncia);
            if (await _unitOfWork.SaveAll())
                return denuncia;
            return null;
        }
        public async Task<bool> DeleteDenuncia(Denuncia denuncia)
        {
            _unitOfWork.DenunciaRepository.Delete(denuncia);
            return await _unitOfWork.SaveAll();
        }
    }
}
