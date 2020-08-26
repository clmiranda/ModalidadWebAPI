using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public partial class Seguimiento : BaseEntity
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaConclusion { get; set; }
        public int CantidadVisitas { get; set; }
        public string Estado { get; set; }
        public virtual User User { get; set; }
        public int? UserId { get; set; }
        //public int IdContratoAdopcion { get; set; }
        //public int? IdReporteSeguimiento { get; set; }
        public virtual ContratoAdopcion ContratoAdopcion { get; set; }
        public int ContratoAdopcionId { get; set; }
        public virtual ICollection<ReporteSeguimiento> ReporteSeguimientos { get; set; }
        //public virtual ReporteSeguimiento ReporteSeguimiento { get; set; }
    }
}
