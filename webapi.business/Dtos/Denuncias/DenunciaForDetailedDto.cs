using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.CasosMascota;

namespace webapi.business.Dtos.Denuncias
{
    public class DenunciaForDetailedDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public virtual CasoMascotaForDetailedDto CasoMascotas { get; set; }
    }
}
