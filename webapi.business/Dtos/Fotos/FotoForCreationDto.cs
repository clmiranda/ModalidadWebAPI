using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace webapi.business.Dtos.Fotos
{
    public class FotoForCreationDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool? IsPrincipal { get; set; }
        public IEnumerable<IFormFile> Archivo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string IdPublico { get; set; }
        public int? MascotaId { get; set; }
        public int? ReporteSeguimientoId { get; set; }
        public FotoForCreationDto()
        {
            FechaCreacion = DateTime.Now;
        }
    }
}
