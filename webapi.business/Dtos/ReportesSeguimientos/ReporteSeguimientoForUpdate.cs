using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using webapi.core.Models;

namespace webapi.business.Dtos.ReportesSeguimientos
{
    public class ReporteSeguimientoForUpdate: BaseEntity
    {
        [Required(ErrorMessage = "Debe ingresar la descripción del actual reporte.")]
        [StringLength(maximumLength: 3000, MinimumLength = 20, ErrorMessage = "La descripción debe contener como mínimo 20 caracteres.")]
        public string Descripcion { get; set; }
        public string Estado { get;}
        public ReporteSeguimientoForUpdate()
        {
            Estado = "Enviado";
        }
    }
}
