using System;
using webapi.core.Models;

namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForUpdateAdmin: BaseEntity
    {
        public DateTime Fecha { get; set; }
        public string Estado { get; }
        public int SeguimientoId { get; set; }
        public ReporteSeguimientoForUpdateAdmin()
        {
            Estado = "Asignado";
        }
    }
}
