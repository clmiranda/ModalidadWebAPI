using System;
using System.Collections.Generic;
using System.Text;
using webapi.core.Models;

namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForUpdateAdmin: BaseEntity
    {
        public DateTime FechaReporte { get; set; }
        public string Estado { get; }
        public ReporteSeguimientoForUpdateAdmin()
        {
            Estado = "Asignado";
        }
    }
}
