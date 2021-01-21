using AutoMapper;
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
        public IEnumerable<ReporteSeguimiento> GetAll()
        {
            var lista = _unitOfWork.ReporteSeguimientoRepository.GetAll();
            return lista;
        }
        public IEnumerable<ReporteSeguimientoForReturn> GetReportesForAdmin(int id)
        {
            var lista = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(_unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.SeguimientoId == id).OrderBy(y => y.FechaRealizada.Date).ToList());
            return lista;
        }
        public IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id)
        {
            var lista = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(_unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.SeguimientoId == id && x.Estado.Equals("Asignado")).ToList().OrderBy(y => y.FechaRealizada.Date));
            return lista;
        }
        public async Task<bool> CreateReporteSeguimiento(ReporteSeguimiento reporte) {
            //var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(reporteSeguimiento.SeguimientoId);
            //var reporte = _mapper.Map<ReporteSeguimiento>(reporteDto);
            //reporte.Seguimiento = seguimiento;
            //reporteDto.FechaAsignada = DateTime.Now;
            reporte.Estado = "Activo";
            //var x = _mapper.Map<ReporteSeguimiento>(reporte);
            _unitOfWork.ReporteSeguimientoRepository.Insert(reporte);
            return await _unitOfWork.SaveAll();
        }
        public async Task<Seguimiento> CreateReporte(ReporteSeguimientoForCreate reporteDto) {
            var seguimiento = await _unitOfWork.SeguimientoRepository.GetById(reporteDto.SeguimientoId);
            var reporte = _mapper.Map<ReporteSeguimiento>(reporteDto);
            reporte.Seguimiento = seguimiento;
            _unitOfWork.ReporteSeguimientoRepository.Insert(reporte);
            seguimiento.ReporteSeguimientos.Add(reporte);
            _unitOfWork.SeguimientoRepository.Update(seguimiento);
            if (await _unitOfWork.SaveAll())
                return seguimiento;
            return null;
        }
        public async Task<bool> VerifyDate(ReporteSeguimientoForUpdateAdmin reporte) {
            var modelo = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporte.Id);
            if (modelo.Seguimiento.FechaInicio.Date<= reporte.FechaReporte.Date && modelo.Seguimiento.FechaConclusion.Date >= reporte.FechaReporte.Date)
            {
                if (!modelo.Seguimiento.ReporteSeguimientos.Any(x=>x.FechaRealizada.Date== reporte.FechaReporte.Date && !x.Estado.Equals("Activo")))
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
            modelo.Observaciones = reporte.Descripcion;
            modelo.Estado = reporte.Estado;
            _unitOfWork.ReporteSeguimientoRepository.Update(modelo);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateReporteSeguimientoAdmin(ReporteSeguimientoForUpdateAdmin reporte)
        {
            var modelo = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporte.Id);
            modelo.FechaRealizada = reporte.FechaReporte.Date;
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
