using System;

namespace webapi.business.Dtos.ContratoRechazo
{
    public class ContratoRechazoForCreateDto
    {
        public string RazonRechazo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int ContratoAdopcionId { get; set; }
        public ContratoRechazoForCreateDto()
        {
            FechaCreacion = DateTime.Now;
        }
    }
}
