using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.SolicitudAdopcionCancelada;
using webapi.business.Dtos.SolicitudAdopcionRechazada;
using webapi.business.Pagination;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class AdopcionService : IAdopcionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISeguimientoService _seguimientoService;
        private readonly List<string> listaEstado;
        public AdopcionService(IUnitOfWork unitOfWork, IMapper mapper, ISeguimientoService seguimientoService)
        {
            _unitOfWork = unitOfWork;
            _seguimientoService = seguimientoService;
            _mapper = mapper;
            listaEstado = new List<string>() { "Aprobado", "Pendiente", "Rechazado", "Cancelado" };
        }
        public async Task<IEnumerable<SolicitudAdopcion>> GetAll()
        {
            return await _unitOfWork.SolicitudAdopcionRepository.GetAll().ToListAsync();
        }
        public async Task<IEnumerable<AdopcionRechazada>> GetAllSolicitudesAdopcionRechazadas()
        {
            return await _unitOfWork.AdopcionRechazadaRepository.GetAll().ToListAsync();
        }
        public async Task<IEnumerable<AdopcionCancelada>> GetAllSolicitudesAdopcionCanceladas()
        {
            return await _unitOfWork.AdopcionCanceladaRepository.GetAll().ToListAsync();
        }
        public async Task<SolicitudAdopcion> GetById(int id)
        {
            return await _unitOfWork.SolicitudAdopcionRepository.GetById(id);
        }
        public async Task<PaginationList<SolicitudAdopcion>> GetAllAdopciones(AdopcionParametros parametros)
        {
            var resul = _unitOfWork.SolicitudAdopcionRepository.GetAll();

            if (string.IsNullOrEmpty(parametros.Filter) || !listaEstado.Contains(parametros.Filter))
                return null;
            if (parametros.Filter == "Aprobado")
                resul = resul.Where(x => x.Estado.Equals("Aprobado"));
            else if (parametros.Filter == "Pendiente")
                resul = resul.Where(x => x.Estado.Equals("Pendiente"));
            else if (parametros.Filter == "Rechazado")
                resul = resul.Where(x => x.Estado.Equals("Rechazado"));
            else if (parametros.Filter == "Cancelado")
                resul = resul.Where(x => x.Estado.Equals("Cancelado"));

            var pagination = await PaginationList<SolicitudAdopcion>.ToPagedList(resul, parametros.PageNumber, parametros.PageSize);
            return pagination;
        }
        public async Task<SolicitudAdopcion> CreateSolicitudAdopcion(SolicitudAdopcionForCreate solicitudAdopcionDto)
        {
            var solicitudAdopcion = _mapper.Map<SolicitudAdopcion>(solicitudAdopcionDto);
            var mascota = await _unitOfWork.MascotaRepository.GetById(solicitudAdopcionDto.MascotaId);
            mascota.Estado = "En Proceso";
            _unitOfWork.SolicitudAdopcionRepository.Insert(solicitudAdopcion);

            if (await _unitOfWork.SaveAll())
                return solicitudAdopcion;
            return null;
        }
        public async Task<bool> UpdateFecha(FechaSolicitudAdopcionForUpdateDto fechaSolicitudAdopcionDto)
        {
            var solicitudAdopcion = await _unitOfWork.SolicitudAdopcionRepository.GetById(fechaSolicitudAdopcionDto.Id);
            var mapped = _mapper.Map(fechaSolicitudAdopcionDto, solicitudAdopcion);
            _unitOfWork.SolicitudAdopcionRepository.Update(mapped);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> CreateSolicitudAdopcionRechazada(SolicitudAdopcionRechazadaForCreateDto solicitudAdopcionRechazadaDto)
        {
            var adopcionRechazada = _mapper.Map<AdopcionRechazada>(solicitudAdopcionRechazadaDto);
            _unitOfWork.AdopcionRechazadaRepository.Insert(adopcionRechazada);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> CreateSolicitudAdopcionCancelada(SolicitudAdopcionCanceladaForCreateDto solicitudAdopcionCanceladaDto)
        {
            var adopcionCancelada = _mapper.Map<AdopcionCancelada>(solicitudAdopcionCanceladaDto);
            _unitOfWork.AdopcionCanceladaRepository.Insert(adopcionCancelada);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> AprobarSolicitudAdopcion(int id)
        {
            var solicitudAdopcion = await _unitOfWork.SolicitudAdopcionRepository.GetById(id);
            solicitudAdopcion.Estado = "Aprobado";
            solicitudAdopcion.Mascota.Estado = "Adoptada";
            _unitOfWork.SolicitudAdopcionRepository.Update(solicitudAdopcion);

            _seguimientoService.CreateSeguimiento(solicitudAdopcion.Id);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> RechazarSolicitudAdopcion(int id, int mascotaId)
        {
            var solicitudAdopcion = await _unitOfWork.SolicitudAdopcionRepository.GetById(id);
            solicitudAdopcion.Mascota = null;
            solicitudAdopcion.Estado = "Rechazado";
            _unitOfWork.SolicitudAdopcionRepository.Update(solicitudAdopcion);

            var mascota = await _unitOfWork.MascotaRepository.GetById(mascotaId);
            mascota.Estado = "Activo";
            _unitOfWork.MascotaRepository.Update(mascota);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> CancelarAdopcion(int id, int mascotaId)
        {
            var solicitudAdopcion = await _unitOfWork.SolicitudAdopcionRepository.GetById(id);

            _unitOfWork.SeguimientoRepository.Delete(solicitudAdopcion.Seguimiento);

            solicitudAdopcion.Mascota = null;
            solicitudAdopcion.Seguimiento = null;
            solicitudAdopcion.Estado = "Cancelado";
            _unitOfWork.SolicitudAdopcionRepository.Update(solicitudAdopcion);

            var mascota = await _unitOfWork.MascotaRepository.GetById(mascotaId);
            mascota.Estado = "Activo";
            _unitOfWork.MascotaRepository.Update(mascota);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> DeleteAllSolicitudAdopcion(int mascotaId)
        {
            var listaSolicitudAdopcion = await _unitOfWork.SolicitudAdopcionRepository.FindByCondition(x => x.MascotaId == mascotaId).ToListAsync();
            foreach (var solicitudAdopcion in listaSolicitudAdopcion)
                _unitOfWork.SolicitudAdopcionRepository.Delete(solicitudAdopcion);
            return true;
        }
    }
}