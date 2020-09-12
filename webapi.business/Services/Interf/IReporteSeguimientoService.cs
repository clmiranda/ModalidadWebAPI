using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IReporteSeguimientoService
    {
        Task<ReporteSeguimiento> GetById(int id);
        Task<IEnumerable<ReporteSeguimiento>> GetAll();
        IEnumerable<ReporteSeguimientoForReturn> GetByCondition(int id);
        Task<bool> CreateReporteSeguimiento(ReporteSeguimientoForCreate reporteDto);
        Task<bool> UpdateReporteSeguimiento(ReporteSeguimientoForUpdate reporte);
        //Task<bool> CreateReporteSeguimiento(ReporteSeguimiento reporteSeguimiento);
    }
}
