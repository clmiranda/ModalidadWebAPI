using System;

namespace webapi.business.Dtos.Adopciones
{
    public class SolicitudAdopcionForCreate
    {
        public string NombreCompleto { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaSolicitudAdopcion { get; }
        public DateTime FechaAdopcion { get; }
        public DateTime FechaCreacion { get; }
        public string Estado { get; }
        public int MascotaId { get; set; }
        public SolicitudAdopcionForCreate()
        {
            FechaSolicitudAdopcion = DateTime.Now;
            FechaAdopcion = DateTime.Now;
            FechaCreacion = DateTime.Now;
            Estado = "Pendiente";
        }
    }
}
