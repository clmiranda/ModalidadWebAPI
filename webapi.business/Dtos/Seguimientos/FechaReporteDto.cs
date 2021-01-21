using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Seguimientos
{
    public class FechaReporteDto
    {
        public int Id { get; set; }
        public string[] RangoFechas { get; set; }
        //public DateTime FechaConclusion { get; set; }
    }
}
