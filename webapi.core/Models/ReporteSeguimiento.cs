using System;
using System.Collections.Generic;

namespace webapi.core.Models
{
    public partial class ReporteSeguimiento : BaseEntity
    {
        public string Observaciones { get; set; }
        //public string EstadoHogarMascota { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
        public virtual Seguimiento Seguimiento { get; set; }
        public int SeguimientoId { get; set; }
        public virtual Foto Foto { get; set; }
    }
}