using System.Collections.Generic;
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
        Task<Seguimiento> GetReportesForAdmin(int id);
        IEnumerable<ReporteSeguimientoForReturn> GetReportesForVoluntario(int id);
        Task<int> VerifyDate(ReporteSeguimientoForUpdateAdmin reporteDto);
        Task<bool> CreateReporteSeguimiento(int id);
        Task<bool> SendReporte(ReporteSeguimientoForUpdate reporteDto);
        Task<bool> UpdateFecha(ReporteSeguimientoForUpdateAdmin reporteDto);
        Task<bool> DeleteReporte(int id);
    }
}
