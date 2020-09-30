using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public partial class ContratoAdopcion : BaseEntity
    {
        //public string NombreCompleto { get; set; }
        //public string Domicilio { get; set; }
        //public string NumeroCelular { get; set; }
        public string RazonAdopcion { get; set; }
        //public string Edad { get; set; }
        public bool TerminosCondiciones { get; set; }
        //public string Ci { get; set; }
        public DateTime FechaSolicitudAdopcion { get; set; }
        public DateTime FechaAdopcion { get; set; }
        //public string RazonRechazoCancelado { get; set; }
        public string Estado { get; set; }
        //public int IdDetalleAdopcion { get; set; }
        //public int IdEstadoAdopcion { get; set; }
        //public int IdUsuario { get; set; }
        //public int IdMascota { get; set; }
        public virtual Mascota Mascota { get; set; }
        public int MascotaId { get; set; }
        public virtual User User { get; set; }
        public int UserId { get; set; }

        //public virtual DetalleAdopcion DetalleAdopcion { get; set; }
        //public virtual EstadoAdopcion EstadoAdopcion { get; set; }
        //public int EstadoAdopcionId { get; set; }
        //public virtual ICollection<Foto> Fotos { get; set; }
        public virtual Seguimiento Seguimiento { get; set; }
        public virtual ContratoRechazo ContratoRechazo { get; set; }
        //public ContratoAdopcion()
        //{
        //    FechaSolicitudAdopcion = DateTime.Now;
        //    FechaAdopcion = DateTime.Now;
        //}
    }
}