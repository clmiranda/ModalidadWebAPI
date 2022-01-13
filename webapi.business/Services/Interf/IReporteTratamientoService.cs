using System.Threading.Tasks;
using webapi.business.Dtos.ReporteTratamientos;
using webapi.core.Models;

namespace webapi.business.Services.Interf
{
    public interface IReporteTratamientoService
    {
        Task<ReporteTratamiento> GetById(int id);
        Task<Mascota> GetAllReporteTratamiento(int id);
        Task<ReporteTratamiento> GetReporteTratamiento(int id);
        Task<bool> CreateReporteTratamiento(ReporteTratamientoForCreateDto reporteTratamientoDto);
        Task<bool> UpdateReporteTratamiento(ReporteTratamientoForUpdateDto reporteTratamientoDto);
        Task<bool> DeleteReporteTratamiento(ReporteTratamiento reporteTratamiento);
        Task<bool> UpdateFecha(FechaReporteTratamientoForUpdateDto fechaReporteTratamiento);
        bool VerifyDateIsRepeated(Mascota mascota, FechaReporteTratamientoForUpdateDto fechaReporteTratamientoDto);
    }
}
