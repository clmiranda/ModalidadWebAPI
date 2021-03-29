using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Adopciones;

namespace webapi.business.Dtos.ContratoRechazo
{
    public class ContratoRechazoForReturnDto
    {
        public int Id { get; set; }
        public string RazonRechazo { get; set; }
        public int ContratoAdopcionId { get; set; }
    }
}
