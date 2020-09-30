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
        IEnumerable<ReporteSeguimientoForReturn> GetReportesForAdmin(int id);
        IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id);
        Task<bool> VerifyDate(ReporteSeguimientoForUpdateAdmin reporte);
        Task<bool> VerifyMaximoReportes(int id);
        Task<bool> VerifyMinimoReportes(int id);
        Task<bool> CreateReporteSeguimiento(ReporteSeguimientoForCreate reporteDto);
        Task<bool> UpdateReporteSeguimientoVoluntario(ReporteSeguimientoForUpdate reporte);
        Task<bool> UpdateReporteSeguimientoAdmin(ReporteSeguimientoForUpdateAdmin reporte);
        Task<bool> DeleteReporte(int id);
        //Task<bool> CreateReporteSeguimiento(ReporteSeguimiento reporteSeguimiento);
    }
}
