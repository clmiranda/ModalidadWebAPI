using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace webapi.business.Dtos.ContratoRechazo
{
    public class ContratoRechazoForCreateDto
    {
        [Required(ErrorMessage ="Debe Ingresar la Razon del Rechazo/Cancelacion")]
        public string RazonRechazo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int ContratoAdopcionId { get; set; }
        public ContratoRechazoForCreateDto()
        {
            FechaCreacion = DateTime.Now;
        }
    }
}
