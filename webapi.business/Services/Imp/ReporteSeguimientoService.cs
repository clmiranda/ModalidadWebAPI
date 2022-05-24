using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            var reporteSeguimiento = await _unitOfWork.ReporteSeguimientoRepository.GetById(id);
            return reporteSeguimiento;
        }
        public async Task<IEnumerable<ReporteSeguimiento>> GetAllReporteSeguimientosForReport()
        {
            var lista = await _unitOfWork.ReporteSeguimientoRepository.GetAll().ToListAsync();
            return lista;
        }
        public async Task<Seguimiento> GetReportesForAdmin(int id)
        {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            return seguimiento;
        }
        public async Task<bool> CreateReporteSeguimiento(int id)
        {
            ReporteSeguimiento reporteSeguimiento = new ReporteSeguimiento
            {
                SeguimientoId = id,
                Estado = "Activo",
                FechaReporte = DateTime.Now,
                FechaCreacion = DateTime.Now
            };
            _unitOfWork.ReporteSeguimientoRepository.Insert(reporteSeguimiento);
            return await _unitOfWork.SaveAll();
        }
        public async Task<int> VerifyDate(ReporteSeguimientoForUpdateAdmin reporteDto)
        {
            var reporteSeguimiento = await _unitOfWork.SeguimientoRepository.GetById(reporteDto.SeguimientoId);
            if (reporteDto.FechaReporte.Date >= reporteSeguimiento.FechaInicio.Date && reporteDto.FechaReporte.Date <= reporteSeguimiento.FechaFin.Date)
            {
                if (reporteSeguimiento.ReporteSeguimientos.Any(x => x.FechaReporte.Date.ToShortDateString() == reporteDto.FechaReporte.Date.ToShortDateString()))
                    return 2;
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
            var reporteSeguimiento = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporteDto.Id);
            var resultado = _mapper.Map(reporteDto, reporteSeguimiento);
            _unitOfWork.ReporteSeguimientoRepository.Update(resultado);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateFechaReporte(ReporteSeguimientoForUpdateAdmin reporteDto)
        {
            var reporteSeguimiento = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporteDto.Id);
            reporteSeguimiento.FechaReporte = reporteDto.FechaReporte.Date;
            reporteSeguimiento.Estado = reporteDto.Estado;
            _unitOfWork.ReporteSeguimientoRepository.Update(reporteSeguimiento);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> DeleteReporte(int id)
        {
            var reporteSeguimiento = await _unitOfWork.ReporteSeguimientoRepository.GetById(id);
            if (reporteSeguimiento != null)
            {
                _unitOfWork.ReporteSeguimientoRepository.Delete(reporteSeguimiento);

                if (reporteSeguimiento.Foto != null)
                    if (await _fotoService.DeleteFotoReporteSeguimiento(reporteSeguimiento.Foto.Id))
                        return await _unitOfWork.SaveAll();
                    else
                        return false;
                else
                    return await _unitOfWork.SaveAll();
            }
            return false;
        }
        public async Task<ReporteSeguimiento> GetByIdNotracking(int id)
        {
            return await _unitOfWork.ReporteSeguimientoRepository.GetByIdNoTraking(id);
        }
    }
}
