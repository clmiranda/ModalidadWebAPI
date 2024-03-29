﻿using System;

namespace webapi.business.Dtos.Fotos
{
    public class FotoForReturnDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool IsPrincipal { get; set; }
        public string IdPublico { get; set; }
        public int? MascotaId { get; set; }
        public int? ReporteSeguimientoId { get; set; }
    }
}
