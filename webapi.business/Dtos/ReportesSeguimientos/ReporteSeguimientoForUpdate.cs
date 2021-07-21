using System;
using webapi.core.Models;

namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForUpdate: BaseEntity
    {
        public string Observaciones { get; set; }
        public string EstadoHogarMascota { get; set; }
        public int SeguimientoId { get; set; }
        public string Estado { get;}
        public ReporteSeguimientoForUpdate()
        {
            Estado = "Enviado";
        }
    }
}
