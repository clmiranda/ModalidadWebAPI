using System;
using System.Collections.Generic;
using webapi.business.Dtos.Fotos;

namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForList
    {
        public int Id { get; set; }
        public string Observaciones { get; set; }
        public string EstadoHogarMascota { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public int SeguimientoId { get; set; }
        public virtual List<FotoForReturnDto> Fotos { get; set; }
    }
}
