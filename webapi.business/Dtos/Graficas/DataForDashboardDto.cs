using System.Collections.Generic;

namespace webapi.business.Dtos.Graficas
{
    public class DataForDashboardDto
    {
        public int ContadorMascotasRegistradas { get; set; }
        public int ContadorDenunciasRegistradas { get; set; }
        public int ContadorReportesSemana { get; set; }
        public int ContadorAdopcionesAprobadas { get; set; }
        public int ContadorAdopcionesRechazadas { get; set; }
        public int ContadorAdopcionesCanceladas { get; set; }
        public int ContadorSeguimientosActuales { get; set; }
        public int ContadorVoluntariosRegistrados { get; set; }
        public List<DataGraficaDto> DataGraficaMascota { get; set; }
        public List<DataGraficaDto> DataGraficaSolicitudAdopcion { get; set; }
    }
}
