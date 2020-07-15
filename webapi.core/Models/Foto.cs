using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public class Foto: BaseEntity
    {
        public string Url { get; set; }
        public bool IsPrincipal { get; set; }
        public string IdPublico { get; set; }
        public DateTime FechaAgregado { get; set; }
        public virtual User User { get; set; }
        public int? UserId { get; set; }
        public virtual Mascota Mascota { get; set; }
        public int? MascotaId { get; set; }
        //public virtual ContratoAdopcion ContratoAdopcion { get; set; }
        //public int? ContratoAdopcionId { get; set; }
    }
}