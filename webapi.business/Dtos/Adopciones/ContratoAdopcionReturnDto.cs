using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Mascotas;
using webapi.core.Models;

namespace webapi.business.Dtos.Adopciones
{
    public class ContratoAdopcionReturnDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Ci { get; set; }
        public string Domicilio { get; set; }
        public string NumeroCelular { get; set; }
        //public string NumeroCelular { get; set; }
        public string Pregunta7 { get; set; }
        //public string Edad { get; set; }
        //public bool TerminosCondiciones { get; set; }
        public DateTime FechaSolicitudAdopcion { get; set; }
        //public DateTime FechaAdopcion { get; set; }
        public string Estado { get; set; }
        public MascotaForReturn Mascota { get; set; }
        //public int UserId { get; set; }
        //public ContratoAdopcionDto()
        //{
        //    FechaAdopcion = DateTime.Now;
        //}
    }
}
