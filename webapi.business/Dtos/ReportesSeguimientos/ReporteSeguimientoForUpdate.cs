using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using webapi.core.Models;

namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForUpdate: BaseEntity
    {
        public string Observaciones { get; set; }
        //public string EstadoMascota { get; set; }
        public string EstadoHogarMascota { get; set; }
        public DateTime Fecha { get; set; }
        public int SeguimientoId { get; set; }
        public string Estado { get;}
        public ReporteSeguimientoForUpdate()
        {
            Estado = "Enviado";
            Fecha = DateTime.Now;
        }
    }
}
