using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Adopciones
{
    public class ContratoAdopcionReturnDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Domicilio { get; set; }
        public string NumeroCelular { get; set; }
        public string RazonAdopcion { get; set; }
        public string Edad { get; set; }
        public bool TerminosCondiciones { get; set; }
        public DateTime FechaSolicitudAdopcion { get; set; }
        public DateTime FechaAdopcion { get; set; }
        public string Estado { get; set; }
        public int MascotaId { get; set; }
        public int UserId { get; set; }
        //public ContratoAdopcionDto()
        //{
        //    FechaAdopcion = DateTime.Now;
        //}
    }
}
