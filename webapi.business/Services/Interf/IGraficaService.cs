using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using webapi.business.Dtos.Graficas;

namespace webapi.business.Services.Interf
{
    public interface IGraficaService
    {
        Task<List<DataGraficaDto>> DatosAdopciones(string filtro);
        Task<List<DataGraficaDto>> DatosMascotas(string filtro);
        Task<List<DataGraficaDto>> DatosReporteSeguimientos(string filtro);
    }
}
