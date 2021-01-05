﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Mascotas;
using webapi.business.Helpers;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class MascotaService : IMascotaService
    {
        private IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MascotaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Mascota> CreateMascota(Mascota mascota)
        {
            //var denuncia = await _unitOfWork.DenunciaRepository.GetById(mascota.DenunciaId);
            //var m = _mapper.Map<Mascota>(mascota);
            mascota.FechaAgregado = DateTime.Now;
            mascota.EstadoSituacion = "Inactivo";
            //m.Denuncia = denuncia;
            //m.DenunciaId = denuncia.Id;
            _unitOfWork.MascotaRepository.Insert(mascota);
            if (await _unitOfWork.SaveAll())
                return mascota;
            return null;
        }

        public async Task<bool> DeleteMascota(Mascota mascota)
        {
            _unitOfWork.MascotaRepository.Delete(mascota);
            return await _unitOfWork.SaveAll();
        }
        public IQueryable<Mascota> FindByCondition(Expression<Func<Mascota, bool>> expression)
        {
            return _unitOfWork.MascotaRepository.FindByCondition(expression).AsQueryable();
        }
        //public IEnumerable<Mascota> GetAllMascotaAdopcion()
        //{
        //    return _unitOfWork.MascotaRepository.GetAllMascotaAdopcion();
        //}

        public async Task<IEnumerable<Mascota>> GetAllMascotas()
        {
            var resul = _unitOfWork.MascotaRepository.GetAll();
            return resul;
        }

        public async Task<PaginationList<Mascota>> GetAllMascotas(MascotaParametros parametros)
        {
            var resul = _unitOfWork.MascotaRepository.GetAll()/*.Include(x=>x.CasoMascotas).ToListAsync()*/;
            //var x = _mapper.Map<IEnumerable<MascotaForAdopcionDto>>(resul);
            //var lista = x.OrderByDescending(x => x.Nombre).AsQueryable();
            //if (String.IsNullOrEmpty(parametros.Busqueda))
            //    parametros.Busqueda = "";
            //if (String.IsNullOrEmpty(parametros.Filter))
            //    parametros.Filter = "";

            if (!String.IsNullOrEmpty(parametros.Busqueda))
                resul = resul.Where(x => x.Nombre.ToLower().Contains(parametros.Busqueda.ToLower()));
            if(parametros.Filter=="Adopcion")
                resul = resul.Where(x => x.Nombre != null && x.ContratoAdopcion==null);

            var pagination = await PaginationList<Mascota>.ToPagedList(resul, parametros.PageNumber, parametros.PageSize);
            //PaginationMascota paginationMascota = new PaginationMascota
            //{
            //    Items = pagination,
            //    CurrentPage = pagination.CurrentPage,
            //    PageSize = pagination.PageSize,
            //    TotalPages = pagination.TotalPages,
            //    TotalCount = pagination.TotalCount
            //};
            return pagination;
            //return await _unitOfWork.MascotaRepository.GetAll();
        }
        public async Task<Mascota> GetMascotaById(int id)
        {
            return await _unitOfWork.MascotaRepository.GetById(id);
        }

        //public Task<Mascota> GetMascotaByIdCaso(int id)
        //{
        //    return _unitOfWork.MascotaRepository.GetMascotaByIdCaso(id);
        //}

        public async Task<Mascota> UpdateMascota(Mascota mascota)
        {
            //_unitOfWork.MascotaRepository.Update(mascota);
            //return await _unitOfWork.SaveAll();
            _unitOfWork.MascotaRepository.Update(mascota);
            if (await _unitOfWork.SaveAll())
                return mascota;
            return null;
        }
        public async Task<bool> ChangeEstado(string estado,int id)
        {
            var mascota = await _unitOfWork.MascotaRepository.GetById(id);
            mascota.EstadoSituacion = estado;
            _unitOfWork.MascotaRepository.Update(mascota);
            return await _unitOfWork.SaveAll();
        }
    }
}
