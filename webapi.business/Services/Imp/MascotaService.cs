using AutoMapper;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MascotaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<MascotaForDetailedDto> GetAll()
        {
            var lista = _unitOfWork.MascotaRepository.GetAll().ToList();
            var mapped = _mapper.Map<IEnumerable<MascotaForDetailedDto>>(lista);
            return mapped;
        }
        public async Task<Mascota> CreateMascota(Mascota mascota)
        {
            mascota.FechaAgregado = DateTime.Now;
            mascota.EstadoSituacion = "Inactivo";
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

        public async Task<PaginationList<Mascota>> GetAllMascotas(MascotaParametros parametros)
        {
            var resul = _unitOfWork.MascotaRepository.GetAll();

            if (!String.IsNullOrEmpty(parametros.Busqueda))
                resul = resul.Where(x => x.Nombre.ToLower().Contains(parametros.Busqueda.ToLower()));
            if (parametros.Filter == "Adopcion")
                    resul = resul.Where(x => x.Nombre != null && x.ContratoAdopcion == null && x.EstadoSituacion == "Activo");

            var pagination = await PaginationList<Mascota>.ToPagedList(resul, parametros.PageNumber, parametros.PageSize);
            return pagination;
        }
        public async Task<Mascota> GetMascotaById(int id)
        {
            return await _unitOfWork.MascotaRepository.GetById(id);
        }
        public async Task<Mascota> UpdateMascota(Mascota mascota)
        {
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
