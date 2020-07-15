using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.CasosMascota
{
    public class CasoMascotaForListDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRescate { get; set; }
        public string TituloDenuncia { get; set; }
    }
}
