using System;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public class ReporteTratamiento : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; }
        [Required]
        [MaxLength(300)]
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        [Required]
        public virtual Mascota Mascota { get; set; }
        public int MascotaId { get; set; }
    }
}
