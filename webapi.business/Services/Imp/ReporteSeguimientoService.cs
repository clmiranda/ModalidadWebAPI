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
        public async Task<IEnumerable<ReporteSeguimiento>> GetAll()
        {
            var lista = await _unitOfWork.ReporteSeguimientoRepository.GetAll();
            return lista;
        }
        public IEnumerable<ReporteSeguimientoForReturn> GetByCondition(int id)
        {
            var lista = _mapper.Map<IEnumerable<ReporteSeguimientoForReturn>>(_unitOfWork.ReporteSeguimientoRepository.FindByCondition(x=>x.SeguimientoId==id).ToList().OrderByDescending(y=>y.FechaReporte));
            return lista;
        }
        public async Task<bool> CreateReporteSeguimiento(ReporteSeguimientoForCreate reporteDto) {
            var reporte = _mapper.Map<ReporteSeguimiento>(reporteDto);
            _unitOfWork.ReporteSeguimientoRepository.Insert(reporte);
            return await _unitOfWork.SaveAll();
        }

        public async Task<bool> UpdateReporteSeguimiento(ReporteSeguimientoForUpdate reporte)
        {
            var modelo = await _unitOfWork.ReporteSeguimientoRepository.GetById(reporte.Id);
            modelo = new ReporteSeguimiento
            {
                Descripcion = reporte.Descripcion,
                Estado = reporte.Estado
            };
            //var modelo = _mapper.Map<ReporteSeguimiento>(reporte);
            _unitOfWork.ReporteSeguimientoRepository.Update(modelo);
            return await _unitOfWork.SaveAll();
        }
        //public async Task<bool> CreateReporteSeguimiento(ReporteSeguimiento reporteSeguimiento)
        //{
        //    _unitOfWork.ReporteSeguimientoRepository.Insert(reporteSeguimiento);
        //    return await _unitOfWork.SaveAll();
        //}
    }
}
