using System;

namespace webapi.business.Dtos.SolicitudAdopcionRechazada
{
    public class SolicitudAdopcionRechazadaForCreateDto
    {
        public string Razon { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int SolicitudAdopcionId { get; set; }
        public SolicitudAdopcionRechazadaForCreateDto()
        {
            FechaCreacion = DateTime.Now;
        }
    }
}
