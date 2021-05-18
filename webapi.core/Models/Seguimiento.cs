using System;
using System.Collections.Generic;

namespace webapi.core.Models
{
    public partial class Seguimiento : BaseEntity
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaConclusion { get; set; }
        public string Estado { get; set; }
        public virtual User User { get; set; }
        public int? UserId { get; set; }
        public virtual ContratoAdopcion ContratoAdopcion { get; set; }
        public int ContratoAdopcionId { get; set; }
        public virtual List<ReporteSeguimiento> ReporteSeguimientos { get; set; }
    }
}
