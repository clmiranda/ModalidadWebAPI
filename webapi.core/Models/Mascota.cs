using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public partial class Mascota : BaseEntity
    {
        public Mascota()
        {
            Fotos = new List<Foto>();
            ReporteTratamientos = new List<ReporteTratamiento>();
        }
        [Required]
        [MaxLength(20)]
        public string Nombre { get; set; }
        [Required]
        [MaxLength(20)]
        public string Sexo { get; set; }
        [MaxLength(100)]
        public string Especie { get; set; }
        [MaxLength(300)]
        public string Caracteristicas { get; set; }
        [MaxLength(300)]
        public string RasgosParticulares { get; set; }
        [MaxLength(50)]
        public string Tamano { get; set; }
        public bool Esterilizado { get; set; }
        [MaxLength(50)]
        public string Edad { get; set; }
        public DateTime FechaCreacion { get; set; }
        [MaxLength(20)]
        public string Estado { get; set; }
        [Required]
        public virtual Denuncia Denuncia { get; set; }
        public int? DenunciaId { get; set; }
        public virtual List<Foto> Fotos { get; set; }
        public virtual List<ReporteTratamiento> ReporteTratamientos { get; set; }
        public virtual SolicitudAdopcion SolicitudAdopcion { get; set; }
    }
}
