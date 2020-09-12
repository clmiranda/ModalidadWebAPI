using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Seguimientos
{
    public class SeguimientoForDetailedDto
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaConclusion { get; set; }
        public int CantidadVisitas { get; set; }
        public int ContratoAdopcionId { get; set; }
        public int UserId { get; set; }
    }
}
