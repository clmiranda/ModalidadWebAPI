using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public partial class CasoMascota : BaseEntity
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaRescate { get; set; }
        public string Estado { get; set; }
        //public int IdDenuncia { get; set; }
        public virtual Denuncia Denuncia { get; set; }
        public int DenunciaId { get; set; }
        public virtual ICollection<Mascota> Mascotas { get; set; }
        //public int IdMascota { get; set; }

        //public virtual Mascota Mascota { get; set; }
    }
}
