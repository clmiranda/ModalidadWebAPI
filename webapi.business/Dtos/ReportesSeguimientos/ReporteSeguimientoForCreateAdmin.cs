using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForCreateAdmin
    {
        public int Id { get; set; }
        public int SeguimientoId { get; set; }
        public string Estado { get; set; }
        //public DateTime FechaAsignada { get; set; }
    }
}
