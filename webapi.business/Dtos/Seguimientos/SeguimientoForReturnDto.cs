using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.Usuario;

namespace webapi.business.Dtos.Seguimientos
{
    public class SeguimientoForReturnDto
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaConclusion { get; set; }
        public int CantidadVisitas { get; set; }
        public virtual ContratoAdopcionForList ContratoAdopcion { get; set; }
        public int ContratoAdopcionId { get; set; }
        public int UserId { get; set; }
        //public virtual IEnumerable<UserForListDto> ListaVoluntarios { get; set; }
    }
}