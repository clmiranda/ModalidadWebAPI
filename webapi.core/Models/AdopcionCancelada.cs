using System;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public class AdopcionCancelada: BaseEntity
    {
        [Required]
        [MaxLength(300)]
        public string Razon { get; set; }
        public DateTime FechaCreacion { get; set; }
        [Required]
        public virtual SolicitudAdopcion SolicitudAdopcion { get; set; }
        public int SolicitudAdopcionId { get; set; }
    }
}