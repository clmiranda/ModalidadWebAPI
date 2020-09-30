using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Denuncias
{
    public class DenunciaForDetailedDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        //public virtual CasoMascotaForListDto CasoMascota { get; set; }
    }
}