using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public class ContratoRechazo : BaseEntity
    {
        public string RazonRechazo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public virtual ContratoAdopcion ContratoAdopcion { get; set; }
        public int ContratoAdopcionId { get; set; }
    }
}
