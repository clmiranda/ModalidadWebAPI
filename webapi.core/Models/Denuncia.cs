﻿using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public partial class Denuncia : BaseEntity
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public virtual ICollection<CasoMascota> CasoMascotas { get; set; }
    }
}