using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public partial class ReporteSeguimiento : BaseEntity
    {
        //public ReporteSeguimiento()
        //{
        //    Seguimientos = new HashSet<Seguimiento>();
        //}
        //public string Descripcion { get; set; }
        public string EstadoMascota { get; set; }
        public string EstadoHogarMascota { get; set; }
        public string Observaciones { get; set; }
        //public DateTime FechaAsignada { get; set; }
        public DateTime Fecha { get; set; }
        //public DateTime FechaReporte { get; set; }
        public string Estado { get; set; }
        //public int IdUsuario { get; set; }
        //public int? IdSeguimiento { get; set; }
        public virtual Seguimiento Seguimiento { get; set; }
        public int SeguimientoId { get; set; }
        public virtual ICollection<Foto> Fotos { get; set; }
        //public virtual ICollection<Seguimiento> Seguimientos { get; set; }
    }
}