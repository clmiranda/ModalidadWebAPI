using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public partial class Seguimiento : BaseEntity
    {
        public Seguimiento()
        {
            ReporteSeguimientos = new List<ReporteSeguimiento>();
        }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; }
        public virtual User User { get; set; }
        public int? UserId { get; set; }
        [Required]
        public virtual SolicitudAdopcion SolicitudAdopcion { get; set; }
        public int SolicitudAdopcionId { get; set; }
        public virtual List<ReporteSeguimiento> ReporteSeguimientos { get; set; }
    }
}
