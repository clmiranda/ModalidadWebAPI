using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Dtos.Seguimientos;
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
            var lista = _unitOfWork.ReporteSeguimientoRepository.GetAll().ToList();
            return lista;
        }
        public IEnumerable<ReporteSeguimiento> GetAll(int id)
        {
            var lista = _unitOfWork.ReporteSeguimientoRepository.FindByCondition(x=>x.SeguimientoId==id).ToList();
            return lista;
        }
        public SeguimientoForReturnDto GetReportesForAdmin(int id)
        {
            var mappeado = _mapper.Map<SeguimientoForReturnDto>(_unitOfWork.SeguimientoRepository.GetById(id).Result);
            return mappeado;
        }
        public IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id)
        {
            var lista = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(_unitOfWork.ReporteSeguimientoRepository.FindByCondition(x => x.SeguimientoId == id && x.Estado.Equals("Asignado")).ToList().OrderBy(y => y.Fecha.Date));
            return lista;
        }
        public async Task<bool> CreateReporteSeguimiento(int id) {
            ReporteSeguimiento r = new ReporteSeguimiento();
            r.SeguimientoId = id;
            r.Estado = "Activo";
            r.Fecha = DateTime.Now;
            //var x = _mapper.Map<ReporteSeguimiento>(reporte);
            _unitOfWork.ReporteSeguimientoRepository.Insert(r);
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
        public async Task<int> VerifyDate(ReporteSeguimientoForUpdateAdmin reporte) {
            var modelo = await _unitOfWork.SeguimientoRepository.GetById(reporte.SeguimientoId);
            if (modelo.FechaInicio.Date<= reporte.Fecha.Date && modelo.FechaConclusion.Date >= reporte.Fecha.Date)
            {
                if (!modelo.ReporteSeguimientos.Any(x=>x.Fecha.Date.ToShortDateString()== reporte.Fecha.Date.ToShortDateString()/* && !x.Estado.Equals("Activo")*/))
                    return 1;

                return 2;
            }
            return 3;
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
            //var modelo = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporte.Id);
            var modelo = _mapper.Map<ReporteSeguimiento>(reporte);
            //modelo.Observaciones = reporte.Descripcion;
            //modelo.Estado = reporte.Estado;
            _unitOfWork.ReporteSeguimientoRepository.Update(modelo);
            return await _unitOfWork.SaveAll();
        }
        public async Task<bool> UpdateReporteSeguimientoAdmin(ReporteSeguimientoForUpdateAdmin reporte)
        {
            var modelo = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporte.Id);
            modelo.Fecha = reporte.Fecha.Date;
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

        public async Task<ReporteSeguimiento> GetByIdNotracking(int id)
        {
            return await _unitOfWork.ReporteSeguimientoRepository.GetByIdNoTraking(id);
        }
    }
}
