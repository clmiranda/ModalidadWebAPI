using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.ReporteTratamientos
{
    public class ReporteTratamientoForReturnDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int MascotaId { get; set; }
    }
}
