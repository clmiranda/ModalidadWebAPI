using System;
using webapi.business.Dtos.Seguimientos;
using webapi.core.Models;

namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForReturn: BaseEntity
    {
        public string Observaciones { get; set; }
        public string Estado { get; set; }
        public DateTime FechaReporte { get; set; }
        public virtual SeguimientoForReturnDto Seguimiento { get; set; }
        public int? SeguimientoId { get; set; }
    }
}