using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Fotos;
using webapi.business.Dtos.ReporteTratamientos;

namespace webapi.business.Dtos.Mascotas
{
    public class MascotaForReturn
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Especie { get; set; }
        public string Caracteristicas { get; set; }
        public string RasgosParticulares { get; set; }
        public string Tamaño { get; set; }
        public bool Esterilizado { get; set; }
        public string Edad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
        public List<ReporteTratamientoForReturnDto> ReporteTratamientos { get; set; }
        public virtual ICollection<FotoForReturnDto> Fotos { get; set; }
    }
}
