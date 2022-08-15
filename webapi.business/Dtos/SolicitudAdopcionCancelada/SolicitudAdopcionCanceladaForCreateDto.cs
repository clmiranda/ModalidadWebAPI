using System;

namespace webapi.business.Dtos.SolicitudAdopcionCancelada
{
    public class SolicitudAdopcionCanceladaForCreateDto
    {
        public string Razon { get; set; }
        public DateTime FechaCreacion { get; }
        public int SolicitudAdopcionId { get; set; }
        public SolicitudAdopcionCanceladaForCreateDto()
        {
            FechaCreacion = DateTime.Now;
        }
    }
}
