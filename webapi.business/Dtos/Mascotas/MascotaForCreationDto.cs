using System;
using System.Collections.Generic;
using System.Text;
using webapi.core.Models;

namespace webapi.business.Dtos.Mascotas
{
    public class MascotaForCreationDto
    {
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Descripcion { get; set; }
        public string Tamaño { get; set; }
        public bool? Esterilizado { get; set; }
        public string Edad { get; set; }
        public DateTime FechaAgregado { get; }
        public string EstadoSituacion { get; }
        public int DenunciaId { get; set; }
        public MascotaForCreationDto()
        {
            FechaAgregado = DateTime.Now;
            EstadoSituacion = "Inactivo";
        }
    }
}
