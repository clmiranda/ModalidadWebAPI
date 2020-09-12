using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.ReportesSeguimientos;
using webapi.business.Dtos.Usuario;

namespace webapi.business.Dtos.Seguimientos
{
    public class SeguimientoForReturnDto
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaConclusion { get; set; }
        public int CantidadVisitas { get; set; }
        public string Estado { get; set; }
        public virtual ContratoAdopcionForList ContratoAdopcion { get; set; }
        public int ContratoAdopcionId { get; set; }
        public virtual UserForDetailedDto User { get; set; }
        public int UserId { get; set; }
        //public virtual ICollection<ReporteSeguimientoForReturn> ReporteSeguimientos { get; set; }
        //public virtual IEnumerable<UserForListDto> ListaVoluntarios { get; set; }
    }
}