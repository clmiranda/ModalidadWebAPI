using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Fotos
{
    public class FotosForDetailedDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime FechaAgregado { get; set; }
        public bool IsPrincipal { get; set; }
    }
}
