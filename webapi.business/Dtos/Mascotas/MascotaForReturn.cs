﻿using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Fotos;

namespace webapi.business.Dtos.Mascotas
{
    public class MascotaForReturn
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Descripcion { get; set; }
        public string Tamaño { get; set; }
        public bool? Esterilizado { get; set; }
        public string Edad { get; set; }
        public DateTime FechaAgregado { get; set; }
        public string EstadoSituacion { get; set; }
        //public virtual CasoMascotaForDetailedDto CasoMascota { get; set; }
        //public int CasoMascotaId { get; set; }
        public virtual ICollection<FotoForReturnDto> Fotos { get; set; }
    }
}
