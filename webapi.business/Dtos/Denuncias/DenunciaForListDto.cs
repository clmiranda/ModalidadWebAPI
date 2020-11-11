using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Mascotas;
using webapi.core.Models;

namespace webapi.business.Dtos.Denuncias
{
    public class DenunciaForListDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public virtual MascotaForDetailedDto Mascota { get; set; }
        public int MascotaId { get; set; }
        //public virtual CasoMascotaForDetailedDto CasoMascota { get; set; }
        //public DenunciaForListDto()
        //{
        //    CasoMascota = new CasoMascotaForDetailedDto();
        //}
    }
}
