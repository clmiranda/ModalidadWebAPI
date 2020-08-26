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
        Task<IEnumerable<ReporteSeguimiento>> GetAll();
        Task<bool> CreateReporteSeguimiento(ReporteSeguimientoForCreate reporteDto);
        //Task<bool> CreateReporteSeguimiento(ReporteSeguimiento reporteSeguimiento);
    }
}
