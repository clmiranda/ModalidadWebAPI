using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Denuncias
{
    public class DenunciaForListDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int IdCasoMascota { get; set; }
    }
}
