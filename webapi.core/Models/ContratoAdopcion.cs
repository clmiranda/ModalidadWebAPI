using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public partial class ContratoAdopcion : BaseEntity
    {
        //datos
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Ci { get; set; }
        public string Domicilio { get; set; }
        public string NumeroCelular { get; set; }

        //preguntas
        public string Pregunta1 { get; set; }
        public string Pregunta2 { get; set; }
        public string Pregunta3 { get; set; }
        public string Pregunta4 { get; set; }
        public string Pregunta5 { get; set; }
        public string Pregunta6 { get; set; }
        public string Pregunta7 { get; set; }

        //parametro opcional
        //public Foto FotoHogarMascota { get; set; }
        //public bool TerminosCondiciones { get; set; }
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
        //public virtual User User { get; set; }
        //public int UserId { get; set; }

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