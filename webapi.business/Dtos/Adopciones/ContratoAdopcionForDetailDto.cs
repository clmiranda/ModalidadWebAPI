using System;
using webapi.business.Dtos.ContratoRechazo;
using webapi.business.Dtos.Mascotas;

namespace webapi.business.Dtos.Adopciones
{
    public class ContratoAdopcionForDetailDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Ci { get; set; }
        public string Domicilio { get; set; }
        public string NumeroCelular { get; set; }
        public string Respuesta1 { get; set; }
        public string Respuesta2 { get; set; }
        public string Respuesta3 { get; set; }
        public string Respuesta4 { get; set; }
        public string Respuesta5 { get; set; }
        public string Respuesta6 { get; set; }
        public string Respuesta7 { get; set; }
        public string Estado { get; set; }
        public DateTime FechaSolicitudAdopcion { get; set; }
        public MascotaForReturn Mascota { get; set; }
        public ContratoRechazoForReturnDto ContratoRechazo { get; set; }
    }
}
