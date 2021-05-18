using System;

namespace webapi.core.Models
{
    public class Foto: BaseEntity
    {
        public string Url { get; set; }
        public bool IsPrincipal { get; set; }
        public string IdPublico { get; set; }
        public DateTime FechaAgregado { get; set; }
        public virtual Mascota Mascota { get; set; }
        public int? MascotaId { get; set; }
        public virtual ReporteSeguimiento ReporteSeguimiento { get; set; }
        public int? ReporteSeguimientoId { get; set; }
    }
}