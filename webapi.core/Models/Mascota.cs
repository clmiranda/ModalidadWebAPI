using System;
using System.Collections.Generic;

namespace webapi.core.Models
{
    public partial class Mascota : BaseEntity
    {
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Especie { get; set; }
        public string Caracteristicas { get; set; }
        public string RasgosParticulares { get; set; }
        public string Tamaño { get; set; }
        public bool? Esterilizado { get; set; }
        public string Edad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
        public virtual Denuncia Denuncia { get; set; }
        public int? DenunciaId { get; set; }
        public virtual List<Foto> Fotos { get; set; }
        public virtual List<ReporteTratamiento> ReporteTratamientos { get; set; }
        public virtual SolicitudAdopcion SolicitudAdopcion { get; set; }
    }
}
