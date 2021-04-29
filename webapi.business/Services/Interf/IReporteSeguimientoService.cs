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
        IEnumerable<ReporteSeguimiento> GetAll();
        SeguimientoForReturnDto GetReportesForAdmin(int id);
        IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id);
        Task<int> VerifyDate(ReporteSeguimientoForUpdateAdmin reporte);
        Task<bool> CreateReporteSeguimiento(int id);
        Task<bool> SendReporte(ReporteSeguimientoForUpdate reporte);
        Task<bool> UpdateFecha(ReporteSeguimientoForUpdateAdmin reporte);
        Task<bool> DeleteReporte(int id);
        //Task<bool> CreateReporteSeguimiento(ReporteSeguimiento reporteSeguimiento);
    }
}
