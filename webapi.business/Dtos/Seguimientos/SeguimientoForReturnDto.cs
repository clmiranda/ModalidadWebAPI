using System;
using System.Collections.Generic;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Dtos.Usuario;

namespace webapi.business.Dtos.Seguimientos
{
    public class SeguimientoForReturnDto
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public virtual SolicitudAdopcionForList SolicitudAdopcion { get; set; }
        public int SolicitudAdopcionId { get; set; }
        public virtual UserForDetailedDto User { get; set; }
        public int UserId { get; set; }
        public virtual List<ReporteSeguimientoForList> ReporteSeguimientos { get; set; }
    }
}