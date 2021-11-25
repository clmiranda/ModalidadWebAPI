using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class ReporteSeguimientoService : IReporteSeguimientoService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ReporteSeguimientoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            var lista = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(_unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.SeguimientoId == id && x.Estado.Equals("Asignado")).ToList().OrderBy(y => y.Fecha.Date));
            return lista;
        }
        public async Task<bool> CreateReporteSeguimiento(int id)
        {
            ReporteSeguimiento reporte = new ReporteSeguimiento
            {
                SeguimientoId = id,
                Estado = "Activo",
                Fecha = DateTime.Now,
                FechaCreacion = DateTime.Now
            };
            _unitOfWork.ReporteSeguimientoRepository.Insert(reporte);
            return await _unitOfWork.SaveAll();
        }
        public async Task<int> VerifyDate(ReporteSeguimientoForUpdateAdmin reporteDto)
        {
            var reporte = await _unitOfWork.SeguimientoRepository.GetById(reporteDto.SeguimientoId);
            if (reporte.FechaInicio.Date <= reporteDto.Fecha.Date && reporte.FechaFin.Date >= reporteDto.Fecha.Date)
            {
                if (reporte.ReporteSeguimientos.Any(x => x.Fecha.Date.ToShortDateString() == reporteDto.Fecha.Date.ToShortDateString()))
                    return 2;
                if (reporteDto.Fecha.Date < DateTime.Now.Date)
                    return 4;
                else
                    return 1;
            }
            return 3;
        }
        public async Task<bool> SendReporte(ReporteSeguimientoForUpdate reporteDto)
        {
            var reporte = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporteDto.Id);
            var resul = _mapper.Map(reporteDto, reporte);
            _unitOfWork.ReporteSeguimientoRepository.Update(resul);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateFecha(ReporteSeguimientoForUpdateAdmin reporteDto)
        {
            var reporte = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporteDto.Id);
            reporte.Fecha = reporteDto.Fecha.Date;
            reporte.Estado = reporteDto.Estado;
            _unitOfWork.ReporteSeguimientoRepository.Update(reporte);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> DeleteReporte(int id)
        {
            var reporte = await _unitOfWork.ReporteSeguimientoRepository.GetById(id);
            _unitOfWork.ReporteSeguimientoRepository.Delete(reporte);
            return await _unitOfWork.SaveAll();
        }
        public async Task<ReporteSeguimiento> GetByIdNotracking(int id)
        {
            return await _unitOfWork.ReporteSeguimientoRepository.GetByIdNoTraking(id);
        }
    }
}
