using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public partial class ReporteSeguimiento : BaseEntity
    {
        [MaxLength(300)]
        public string Observaciones { get; set; }
        public DateTime FechaReporte { get; set; }
        public DateTime FechaCreacion { get; set; }
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; }
        [Required]
        public virtual Seguimiento Seguimiento { get; set; }
        public int SeguimientoId { get; set; }
        public virtual Foto Foto { get; set; }
    }
}