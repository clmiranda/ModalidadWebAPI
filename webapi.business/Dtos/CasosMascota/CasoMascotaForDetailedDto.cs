﻿using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Denuncias;
using webapi.business.Dtos.Mascotas;

namespace webapi.business.Dtos.CasosMascota
{
    public class CasoMascotaForDetailedDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRescate { get; set; }
        public string Estado { get; set; }
        public int DenunciaId { get; set; }
        public virtual MascotaForDetailedDto Mascota { get; set; }
        //public virtual DenunciaForDetailedDto Denuncia { get; set; }
        //public virtual ICollection<Mascota> Mascotas { get; set; }
    }
}