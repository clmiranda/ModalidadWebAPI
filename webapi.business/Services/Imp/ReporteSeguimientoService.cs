﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Services.Interf;
using webapi.core.Models;
using webapi.data.Repositories.Interf;

namespace webapi.business.Services.Imp
{
    public class ReporteSeguimientoService: IReporteSeguimientoService
    {
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork;
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
        public async Task<IEnumerable<ReporteSeguimiento>> GetAll()
        {
            var lista = await _unitOfWork.ReporteSeguimientoRepository.GetAll();
            return lista;
        }
        public IEnumerable<ReporteSeguimientoForReturn> GetReportesForAdmin(int id)
        {
            var lista = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(_unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.SeguimientoId == id).OrderBy(y => y.FechaReporte.Date).ToList());
            return lista;
        }
        public IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id)
        {
            var lista = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(_unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.SeguimientoId == id && x.Estado.Equals("Asignado")).ToList().OrderBy(y => y.FechaReporte.Date));
            return lista;
        }
        public async Task<bool> CreateReporteSeguimiento(ReporteSeguimientoForCreate reporteDto) {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(reporteDto.SeguimientoId);
            var reporte = _mapper.Map<ReporteSeguimiento>(reporteDto);
            reporte.Seguimiento = seguimiento;
            _unitOfWork.ReporteSeguimientoRepository.Insert(reporte);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> VerifyDate(ReporteSeguimientoForUpdateAdmin reporte) {
            var modelo = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporte.Id);
            if (modelo.Seguimiento.FechaInicio.Date<= reporte.FechaReporte.Date && modelo.Seguimiento.FechaConclusion.Date >= reporte.FechaReporte.Date)
            {
                if (!modelo.Seguimiento.ReporteSeguimientos.Any(x=>x.FechaReporte.Date== reporte.FechaReporte.Date && !x.Estado.Equals("Activo")))
                    return true;

                return false;
            }
            return false;
        }
        public async Task<bool> VerifyMaximoReportes(int id)
        {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            if (seguimiento.ReporteSeguimientos.Count()>=10)
                return false;

            return true;
        }
        public async Task<bool> VerifyMinimoReportes(int id)
        {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(id);
            if (seguimiento.ReporteSeguimientos.Count() <= 3)
                return false;

            return true;
        }

        public async Task<bool> UpdateReporteSeguimientoVoluntario(ReporteSeguimientoForUpdate reporte)
        {
            var modelo = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporte.Id);
            modelo.Descripcion = reporte.Descripcion;
            modelo.Estado = reporte.Estado;
            _unitOfWork.ReporteSeguimientoRepository.Update(modelo);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateReporteSeguimientoAdmin(ReporteSeguimientoForUpdateAdmin reporte)
        {
            var modelo = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporte.Id);
            modelo.FechaReporte = reporte.FechaReporte.Date;
            modelo.Estado = reporte.Estado;
            _unitOfWork.ReporteSeguimientoRepository.Update(modelo);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> DeleteReporte(int id)
        {
            var reporte = await _unitOfWork.ReporteSeguimientoRepository.GetById(id);
            _unitOfWork.ReporteSeguimientoRepository.Delete(reporte);
            return await _unitOfWork.SaveAll();
        }
    }
}
