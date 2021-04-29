﻿using System;
using System.Collections.Generic;
using System.Text;
using webapi.core.Models;

namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForCreate: BaseEntity
    {
        //public string Descripcion { get; set; }
        //public DateTime FechaReporte { get; }
        public string Estado { get; }
        public DateTime Fecha { get; set; }
        public DateTime FechaCreacion { get; set; }
        public virtual Seguimiento Seguimiento { get; set; }
        public int SeguimientoId { get; set; }
        public ReporteSeguimientoForCreate()
        {
            //FechaReporte = DateTime.Now;
            Estado = "Activo";
            Fecha = DateTime.Now;
            FechaCreacion = DateTime.Now;
        }
    }
}
