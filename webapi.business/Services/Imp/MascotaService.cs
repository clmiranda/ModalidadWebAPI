using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using webapi.business.Dtos.Mascotas;
using webapi.business.Pagination;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class MascotaService : IMascotaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly List<string> listaEstado;
        public MascotaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            listaEstado = new List<string> { "Activo", "Inactivo", "En Proceso", "Adoptada" };
        }
        public async Task<Mascota> GetMascotaById(int id)
        {
            return await _unitOfWork.MascotaRepository.GetById(id);
        }
        public async Task<IEnumerable<Mascota>> GetAllMascotasForReport()
        {
            var lista = await _unitOfWork.MascotaRepository.GetAll().ToListAsync();
            return lista;
        }
        public async Task<Mascota> CreateMascota(MascotaForCreateDto dto)
        {
            var mascota = _mapper.Map<Mascota>(dto);
            _unitOfWork.MascotaRepository.Insert(mascota);
            if (await _unitOfWork.SaveAll())
                return mascota;
            return null;
        }
        public async Task<Mascota> UpdateMascota(MascotaForUpdateDto dto)
        {
            var mascota = await _unitOfWork.MascotaRepository.GetById(dto.Id);
            var mapped = _mapper.Map(dto, mascota);
            _unitOfWork.MascotaRepository.Update(mapped);
            if (await _unitOfWork.SaveAll())
                return mapped;
            return null;
        }
        public async Task<PaginationList<Mascota>> GetAllMascotasForAdopcion(MascotaForAdopcionParametros parametros)
        {
            var resultado = _unitOfWork.MascotaRepository.GetAll();

            if (!String.IsNullOrEmpty(parametros.Busqueda))
                resultado = resultado.Where(x => x.Nombre.ToLower().Contains(parametros.Busqueda.ToLower()));
            if (parametros.Filter == "Adopcion")
                    resultado = resultado.Where(x => x.Nombre != null && (x.SolicitudAdopcion.Estado.Equals("Rechazado") || x.SolicitudAdopcion.Estado.Equals("Cancelado") || x.SolicitudAdopcion == null) && x.Estado == "Activo");

            resultado = resultado.OrderByDescending(x => x.Id);
            var pagination = await PaginationList<Mascota>.ToPagedList(resultado, parametros.PageNumber, parametros.PageSize);
            return pagination;
        }
        public async Task<PaginationList<Mascota>> GetAllMascotas(MascotaParametros parametros)
        {
            var resultado = _unitOfWork.MascotaRepository.GetAll();
            
            if (!String.IsNullOrEmpty(parametros.Busqueda))
                resultado = resultado.Where(x => x.Nombre.ToLower().Contains(parametros.Busqueda.ToLower()) || x.Denuncia.Titulo.ToLower().Contains(parametros.Busqueda.ToLower()));
            if (parametros.Filter.Equals("Activo"))
                resultado = resultado.Where(x => x.Denuncia != null);
            else
                resultado = resultado.Where(x => x.Denuncia == null);

            resultado = resultado.OrderByDescending(x => x.Id);
            var pagination = await PaginationList<Mascota>.ToPagedList(resultado, parametros.PageNumber, parametros.PageSize);
            return pagination;
        }
        public async Task<bool> ChangeEstado(string estado,int id)
        {
            if (string.IsNullOrEmpty(estado) || !listaEstado.Contains(estado))
                return false;
            var mascota = await _unitOfWork.MascotaRepository.GetById(id);
            mascota.Estado = estado;
            _unitOfWork.MascotaRepository.Update(mascota);
            return await _unitOfWork.SaveAll();
        }
        public IQueryable<Mascota> FindByCondition(Expression<Func<Mascota, bool>> expression)
        {
            return _unitOfWork.MascotaRepository.FindByCondition(expression).AsQueryable();
        }
    }
}
