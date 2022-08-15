using System.Collections.Generic;
using System.Threading.Tasks;
using webapi.business.Dtos.Graficas;

namespace webapi.business.Services.Interf
{
    public interface IGraficaService
    {
        Task<List<DataGraficaDto>> DatosAdopciones(string[] fechas);
        Task<List<DataGraficaDto>> DatosMascotas(string[] fechas);
        Task<List<DataGraficaDto>> DatosReporteSeguimientos(string[] fechas);
        Task<DataForDashboardDto> GetDataForDashboard();
    }
}
