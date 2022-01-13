using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class ReporteSeguimientoService : IReporteSeguimientoService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFotoService _fotoService;
        public ReporteSeguimientoService(IUnitOfWork unitOfWork, IMapper mapper,
            IFotoService fotoService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fotoService = fotoService;
        }
        public async Task<ReporteSeguimiento> GetById(int id)
        {
            var reporte = await _unitOfWork.ReporteSeguimientoRepository.GetById(id);
            return reporte;
        }
        public IEnumerable<ReporteSeguimiento> GetAll()
        {
            var lista = _unitOfWork.ReporteSeguimientoRepository.GetAll().ToList();
            return lista;
        }
        public async Task<Seguimiento> GetReportesForAdmin(int id)
        {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            return seguimiento;
        }
        public IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id)
        {
            var lista = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(_unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.SeguimientoId == id && x.Estado.Equals("Asignado")).ToList().OrderBy(y => y.FechaReporte.Date));
            return lista;
        }
        public async Task<bool> CreateReporteSeguimiento(int id)
        {
            ReporteSeguimiento reporte = new ReporteSeguimiento
            {
                SeguimientoId = id,
                Estado = "Activo",
                FechaReporte = DateTime.Now,
                FechaCreacion = DateTime.Now
            };
            _unitOfWork.ReporteSeguimientoRepository.Insert(reporte);
            return await _unitOfWork.SaveAll();
        }
        public async Task<int> VerifyDate(ReporteSeguimientoForUpdateAdmin reporteDto)
        {
            var reporte = await _unitOfWork.SeguimientoRepository.GetById(reporteDto.SeguimientoId);
            if (reporte.FechaInicio.Date <= reporteDto.FechaReporte.Date && reporte.FechaFin.Date >= reporteDto.FechaReporte.Date)
            {
                if (reporte.ReporteSeguimientos.Any(x => x.FechaReporte.Date.ToShortDateString() == reporteDto.FechaReporte.Date.ToShortDateString()))
                    return 2;
                if (reporteDto.FechaReporte.Date < DateTime.Now.Date)
                    return 4;
                else
                    return 1;
            }
            return 3;
        }
        public async Task<Seguimiento> UpdateRangoFechasSeguimiento(RangoFechaSeguimientoDto rangoFechaSeguimiento)
        {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(rangoFechaSeguimiento.Id);
            seguimiento.FechaInicio = Convert.ToDateTime(rangoFechaSeguimiento.RangoFechas[0]);
            seguimiento.FechaFin = Convert.ToDateTime(rangoFechaSeguimiento.RangoFechas[1]);
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            if (await _unitOfWork.SaveAll())
                return seguimiento;
            return null;
        }
        public async Task<bool> SendReporte(ReporteSeguimientoForUpdate reporteDto)
        {
            var reporte = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporteDto.Id);
            var resul = _mapper.Map(reporteDto, reporte);
            _unitOfWork.ReporteSeguimientoRepository.Update(resul);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateFechaReporte(ReporteSeguimientoForUpdateAdmin reporteDto)
        {
            var reporte = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporteDto.Id);
            reporte.FechaReporte = reporteDto.FechaReporte.Date;
            reporte.Estado = reporteDto.Estado;
            _unitOfWork.ReporteSeguimientoRepository.Update(reporte);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> DeleteReporte(int id)
        {
            var reporte = await _unitOfWork.ReporteSeguimientoRepository.GetById(id);
            if (reporte != null)
            {
                _unitOfWork.ReporteSeguimientoRepository.Delete(reporte);

                if (await _fotoService.DeleteFotoReporteSeguimiento(reporte.Foto.Id))
                    return await _unitOfWork.SaveAll();

                return false;
            }
            return false;
        }
        public async Task<ReporteSeguimiento> GetByIdNotracking(int id)
        {
            return await _unitOfWork.ReporteSeguimientoRepository.GetByIdNoTraking(id);
        }
    }
}
