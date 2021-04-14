using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Adopciones
{
    public class ContratoAdopcionForCreate
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
        public DateTime FechaSolicitudAdopcion { get; }
        public DateTime FechaAdopcion { get; }
        //public string RazonRechazoCancelado { get; set; }
        public string Estado { get; }
        public int MascotaId { get; set; }
        public ContratoAdopcionForCreate()
        {
            FechaSolicitudAdopcion = DateTime.Now;
            FechaAdopcion = DateTime.Now;
            Estado = "Pendiente";
        }
    }
}
