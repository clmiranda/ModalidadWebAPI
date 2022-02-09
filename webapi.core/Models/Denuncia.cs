using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public partial class Denuncia : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; }
        [Required]
        [MaxLength(300)]
        public string Descripcion { get; set; }
        public virtual Mascota Mascota { get; set; }
    }
}