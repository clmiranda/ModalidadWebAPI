using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Dtos.Seguimientos;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IReporteSeguimientoService
    {
        Task<ReporteSeguimiento> GetById(int id);
        Task<ReporteSeguimiento> GetByIdNotracking(int id);
        Task<Seguimiento> CreateReporte(ReporteSeguimientoForCreate reporteDto);
        IEnumerable<ReporteSeguimiento> GetAll();
        SeguimientoForReturnDto GetReportesForAdmin(int id);
        IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id);
        Task<int> VerifyDate(ReporteSeguimientoForUpdateAdmin reporte);
        Task<bool> VerifyMaximoReportes(int id);
        Task<bool> VerifyMinimoReportes(int id);
        Task<bool> CreateReporteSeguimiento(int id);
        Task<bool> UpdateReporteSeguimientoVoluntario(ReporteSeguimientoForUpdate reporte);
        Task<bool> UpdateReporteSeguimientoAdmin(ReporteSeguimientoForUpdateAdmin reporte);
        Task<bool> DeleteReporte(int id);
        //Task<bool> CreateReporteSeguimiento(ReporteSeguimiento reporteSeguimiento);
    }
}
