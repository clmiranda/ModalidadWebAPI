using System;

namespace webapi.business.Dtos.ReporteTratamientos
{
    public class ReporteTratamientoForUpdateDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int MascotaId { get; set; }
    }
}
