using System;
using System.Collections.Generic;

namespace webapi.core.Models
{
    public partial class Seguimiento : BaseEntity
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public virtual User User { get; set; }
        public int? UserId { get; set; }
        public virtual SolicitudAdopcion SolicitudAdopcion { get; set; }
        public int SolicitudAdopcionId { get; set; }
        public virtual List<ReporteSeguimiento> ReporteSeguimientos { get; set; }
    }
}
