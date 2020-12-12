using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Denuncias;
using webapi.business.Helpers;
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
        public async  Task<PaginationList<Denuncia>> GetAllDenuncias(DenunciaParametros parametros)
        {
            var resul= _unitOfWork.DenunciaRepository.GetAll()/*.Include(x=>x.CasoMascotas).ToListAsync()*/;
            //var x = _mapper.Map<IEnumerable<DenunciaForListDto>>(resul);
            var lista = resul.OrderByDescending(x => x.Titulo).AsQueryable();
            if (String.IsNullOrEmpty(parametros.Busqueda))
                parametros.Busqueda = "";
            lista = lista.Where(x=>x.Titulo.ToLower().Contains(parametros.Busqueda.ToLower())|| x.Descripcion.ToLower().Contains(parametros.Busqueda.ToLower()));
            var pagination= await PaginationList<Denuncia>.ToPagedList(lista, parametros.PageNumber, parametros.PageSize);
            //PaginationDenuncia paginationDenuncia = new PaginationDenuncia
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
        public async Task<Denuncia> GetDenunciaById(int id)
        {
            var obj= await _unitOfWork.DenunciaRepository.GetById(id);
            return obj;
        }
        public async Task<Denuncia> CreateDenuncia(Denuncia denuncia)
        {
            //Mascota m = new Mascota
            //{
            //    FechaAgregado = DateTime.Now.Date,
            //    EstadoSituacion = "Inactivo"
            //};
            //denuncia.Mascota=m;
            _unitOfWork.DenunciaRepository.Insert(denuncia);
            //_unitOfWork.MascotaRepository.Insert(m);
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
