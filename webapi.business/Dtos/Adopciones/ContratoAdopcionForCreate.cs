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
        public string Pregunta1 { get; set; }
        public string Pregunta2 { get; set; }
        public string Pregunta3 { get; set; }
        public string Pregunta4 { get; set; }
        public string Pregunta5 { get; set; }
        public string Pregunta6 { get; set; }
        public string Pregunta7 { get; set; }
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
