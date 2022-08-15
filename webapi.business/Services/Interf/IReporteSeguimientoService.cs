using Microsoft.AspNetCore.Http;
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
        Task<IEnumerable<ReporteSeguimiento>> GetAllReporteSeguimientosForReport();
        Task<Seguimiento> GetReportesForAdmin(int id);
        Task<int> VerifyDate(ReporteSeguimientoForUpdateAdmin reporteSeguimientoDto);
        Task<Seguimiento> UpdateRangoFechasSeguimiento(RangoFechaSeguimientoDto rangoFechaSeguimiento);
        Task<bool> CreateReporteSeguimiento(int id);
        Task<bool> SendReporte(ReporteSeguimientoForUpdate reporteDto, IFormFile Foto);
        Task<bool> UpdateFechaReporte(ReporteSeguimientoForUpdateAdmin reporteDto);
        Task<bool> DeleteReporte(int id);
    }
}
