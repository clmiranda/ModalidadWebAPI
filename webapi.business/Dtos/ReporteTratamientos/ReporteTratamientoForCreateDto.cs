﻿using System;

namespace webapi.business.Dtos.ReporteTratamientos
{
    public class ReporteTratamientoForCreateDto
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; }
        public int MascotaId { get; set; }
        public ReporteTratamientoForCreateDto()
        {
            FechaCreacion = DateTime.Now;
        }
    }
}
