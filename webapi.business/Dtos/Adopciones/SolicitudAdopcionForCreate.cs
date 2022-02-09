using System;

namespace webapi.business.Dtos.Adopciones
{
    public class SolicitudAdopcionForCreate
    {
        public string NombreCompleto { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }

        public string Respuesta1 { get; set; }
        public string Respuesta2 { get; set; }
        public string Respuesta3 { get; set; }
        public string Respuesta4 { get; set; }
        public string Respuesta5 { get; set; }
        public string Respuesta6 { get; set; }
        public string Respuesta7 { get; set; }
        public DateTime FechaSolicitudAdopcion { get; }
        public DateTime FechaAdopcion { get; }
        public DateTime FechaCreacion { get; set; }
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
