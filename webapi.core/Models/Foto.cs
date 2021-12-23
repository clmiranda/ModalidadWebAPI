using System;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public class Foto: BaseEntity
    {
        [Required]
        [MaxLength(300)]
        public string Url { get; set; }
        [Required]
        public bool IsPrincipal { get; set; }
        [Required]
        [MaxLength(300)]
        public string IdPublico { get; set; }
        public DateTime FechaCreacion { get; set; }
        public virtual Mascota Mascota { get; set; }
        public int? MascotaId { get; set; }
        public virtual ReporteSeguimiento ReporteSeguimiento { get; set; }
        public int? ReporteSeguimientoId { get; set; }
    }
}