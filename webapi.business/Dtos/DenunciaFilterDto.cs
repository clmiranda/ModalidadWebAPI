using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos
{
    public class DenunciaFilterDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
    }
}
