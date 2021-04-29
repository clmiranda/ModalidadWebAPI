using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.ReporteTratamientos;
using webapi.business.Services.Imp;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IReporteTratamientoService
    {
        List<ReporteTratamiento> GetAll(int id);
        Task<ReporteTratamiento> GetReporteTratamiento(int id);
        Task<bool> CreateReporteTratamiento(ReporteTratamientoForCreateDto reporteTratamientoDto);
        Task<bool> UpdateReporteTratamiento(ReporteTratamientoForUpdateDto reporteTratamientoDto);
        Task<bool> DeleteReporteTratamiento(ReporteTratamiento reporteTratamiento);
    }
}
