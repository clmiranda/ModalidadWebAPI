using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Fotos
{
    public class FotoForCreationDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool? IsPrincipal { get; set; }
        //Mismo Name 'File' en el input de la vista
        public IEnumerable<IFormFile> Archivo { get; set; }
        public DateTime FechaAgregado { get; set; }
        public string IdPublico { get; set; }
        public int? MascotaId { get; set; }
        public int? ReporteSeguimientoId { get; set; }
        public FotoForCreationDto()
        {
            FechaAgregado = DateTime.Now;
        }
    }
}
