﻿using System;

namespace webapi.core.Models
{
    public class AdopcionCancelada: BaseEntity
    {
        public string Razon { get; set; }
        public DateTime FechaCreacion { get; set; }
        public virtual SolicitudAdopcion SolicitudAdopcion { get; set; }
        public int SolicitudAdopcionId { get; set; }
    }
}