using AutoMapper;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
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
using Microsoft.Extensions.Options;
using webapi.business.Helpers;

namespace webapi.business.Services.Imp
{
    public class AdopcionService : IAdopcionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISeguimientoService _seguimientoService;
        private readonly Cloudinary _cloudinary;
        private readonly IOptions<ConfigurationCloudinary> _cloudinaryConfig;
        private readonly List<string> listaEstado;
        public AdopcionService(IUnitOfWork unitOfWork, IMapper mapper, ISeguimientoService seguimientoService, IOptions<ConfigurationCloudinary> cloudinaryConfig)
        {
            _unitOfWork = unitOfWork;
            _seguimientoService = seguimientoService;
            _mapper = mapper;
            listaEstado = new List<string>() { "Aprobado", "Pendiente", "Rechazado", "Cancelado" };
            _cloudinaryConfig = cloudinaryConfig;
            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
        );
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<IEnumerable<SolicitudAdopcion>> GetAllAdopcionesForReport()
        {
            return await _unitOfWork.SolicitudAdopcionRepository.GetAll().ToListAsync();
        }
        public async Task<SolicitudAdopcion> GetById(int id)
        {
            return await _unitOfWork.SolicitudAdopcionRepository.GetById(id);
        }
        public async Task<PaginationList<SolicitudAdopcion>> GetAllAdopciones(AdopcionParametros parametros)
        {
            var resultado = _unitOfWork.SolicitudAdopcionRepository.GetAll().OrderBy(x => x.FechaAdopcion);

            if (string.IsNullOrEmpty(parametros.Filter) || !listaEstado.Contains(parametros.Filter))
                return null;
            if (parametros.Filter.Equals("Aprobado"))
                resultado = (IOrderedQueryable<SolicitudAdopcion>)resultado.Where(x => x.Estado.Equals("Aprobado"));
            else if (parametros.Filter.Equals("Pendiente"))
                resultado = (IOrderedQueryable<SolicitudAdopcion>)resultado.Where(x => x.Estado.Equals("Pendiente"));
            else if (parametros.Filter.Equals("Rechazado"))
                resultado = (IOrderedQueryable<SolicitudAdopcion>)resultado.Where(x => x.Estado.Equals("Rechazado"));
            else if (parametros.Filter.Equals("Cancelado"))
                resultado = (IOrderedQueryable<SolicitudAdopcion>)resultado.Where(x => x.Estado.Equals("Cancelado"));

            resultado = resultado.OrderByDescending(x => x.Id);
            var pagination = await PaginationList<SolicitudAdopcion>.ToPagedList(resultado, parametros.PageNumber, parametros.PageSize);
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
        public async Task<bool> CreateAdopcionPresencial(AdopcionPresencialForCreateDto adopcionPresencial)
        {
            var adopcion = _mapper.Map<SolicitudAdopcion>(adopcionPresencial);
            var mascota = await _unitOfWork.MascotaRepository.GetById(adopcionPresencial.MascotaId);
            mascota.Estado = "Adoptada";
            _unitOfWork.SolicitudAdopcionRepository.Insert(adopcion);
            if (!await _unitOfWork.SaveAll())
                return false;

            _seguimientoService.CreateSeguimiento(adopcion.Id);
            return await _unitOfWork.SaveAll();
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

            foreach (var reporteSeguimiento in solicitudAdopcion.Seguimiento.ReporteSeguimientos)
            {
                if (reporteSeguimiento.Foto != null)
                    _unitOfWork.FotoRepository.Delete(reporteSeguimiento.Foto);
            }
            if (solicitudAdopcion.ContratoAdopcion != null)
            {
                var parametros = new DeletionParams(solicitudAdopcion.ContratoAdopcion.IdPublico);
                var resultado = _cloudinary.Destroy(parametros);

                if (resultado.Result.Equals("ok"))
                    _unitOfWork.ContratoAdopcionRepository.Delete(solicitudAdopcion.ContratoAdopcion);
            }

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

        public async Task<bool> UploadContratoAdopcion(int idAdopcion, ContratoAdopcionDto contratoAdopcion)
        {
            var adopcionRepo = await _unitOfWork.SolicitudAdopcionRepository.GetById(idAdopcion);
            var documento = contratoAdopcion.Archivo;
            ImageUploadResult resultUpload = new ImageUploadResult();
            using (var stream = documento.OpenReadStream())
            {
                var parametros = new ImageUploadParams()
                {
                    File = new FileDescription(documento.Name, stream)
                };
                resultUpload = _cloudinary.Upload(parametros);
            }
            contratoAdopcion.Url = resultUpload.SecureUrl.ToString();
            contratoAdopcion.IdPublico = resultUpload.PublicId;
            ContratoAdopcion contratoAdopcionMap = _mapper.Map<ContratoAdopcion>(contratoAdopcion);
            adopcionRepo.ContratoAdopcion = contratoAdopcionMap;
            _unitOfWork.ContratoAdopcionRepository.Insert(contratoAdopcionMap);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> DeleteAllSolicitudAdopcion(int mascotaId)
        {
            var listaSolicitudAdopcion = await _unitOfWork.SolicitudAdopcionRepository.FindByCondition(x => x.MascotaId == mascotaId).ToListAsync();
            foreach (var solicitudAdopcion in listaSolicitudAdopcion)
            {
                if (solicitudAdopcion.ContratoAdopcion != null)
                    _unitOfWork.ContratoAdopcionRepository.Delete(solicitudAdopcion.ContratoAdopcion);

                if (solicitudAdopcion != null)
                    _unitOfWork.SolicitudAdopcionRepository.Delete(solicitudAdopcion);

                if (solicitudAdopcion.Seguimiento != null) {
                    foreach (var reporte in solicitudAdopcion.Seguimiento.ReporteSeguimientos)
                    {
                        if (reporte.Foto != null)
                            _unitOfWork.FotoRepository.Delete(reporte.Foto);
                        if (reporte != null)
                            _unitOfWork.ReporteSeguimientoRepository.Delete(reporte);
                    }
                    _unitOfWork.SeguimientoRepository.Delete(solicitudAdopcion.Seguimiento);
                }
            }
            return true;
        }
        public IQueryable<AdopcionRechazada> FindByConditionAdopcionRechazada(Expression<Func<AdopcionRechazada, bool>> expression)
        {
            return _unitOfWork.AdopcionRechazadaRepository.FindByCondition(expression).AsQueryable();
        }
        public IQueryable<AdopcionCancelada> FindByConditionAdopcionCancelada(Expression<Func<AdopcionCancelada, bool>> expression)
        {
            return _unitOfWork.AdopcionCanceladaRepository.FindByCondition(expression).AsQueryable();
        }
    }
}