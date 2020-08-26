using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Mascotas;

namespace webapi.business.Dtos.Adopciones
{
    public class ContratoAdopcionForList
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Domicilio { get; set; }
        public string NumeroCelular { get; set; }
        public string RazonAdopcion { get; set; }
        public string Edad { get; set; }
        public DateTime FechaSolicitudAdopcion { get; set; }
        public DateTime FechaAdopcion { get; set; }
        public string Estado { get; set; }
        public MascotaForReturn Mascota { get; set; }
    }
}
