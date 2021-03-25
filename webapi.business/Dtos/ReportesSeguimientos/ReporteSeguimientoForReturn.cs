using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Seguimientos;
using webapi.business.Dtos.Usuario;
using webapi.core.Models;

namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForReturn: BaseEntity
    {
        public string Observaciones { get; set; }
        //public string EstadoMascota { get; set; }
        public string EstadoHogarMascota { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRealizada { get; set; }
        //public DateTime FechaAsignada { get; set; }
        public virtual SeguimientoForReturnDto Seguimiento { get; set; }
        public int? SeguimientoId { get; set; }
    }
}