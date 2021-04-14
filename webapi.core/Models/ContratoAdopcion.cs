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
        public string Respuesta1 { get; set; }
        public string Respuesta2 { get; set; }
        public string Respuesta3 { get; set; }
        public string Respuesta4 { get; set; }
        public string Respuesta5 { get; set; }
        public string Respuesta6 { get; set; }
        public string Respuesta7 { get; set; }

        public DateTime FechaSolicitudAdopcion { get; set; }
        public DateTime FechaAdopcion { get; set; }
        public string Estado { get; set; }
        public virtual Mascota Mascota { get; set; }
        public int? MascotaId { get; set; }
        public virtual Seguimiento Seguimiento { get; set; }
        public virtual ContratoRechazo ContratoRechazo { get; set; }
    }
}