using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public partial class Mascota : BaseEntity
    {
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Descripcion { get; set; }
        public string Tamaño { get; set; }
        public bool? Esterilizado { get; set; }
        public string Edad { get; set; }
        public DateTime FechaAgregado { get; set; }
        public string EstadoSituacion { get; set; }
        //public byte[] Imagen { get; set; }
        //public int IdTipoMascota { get; set; }
        //public int IdCasoMascota { get; set; }

        //public virtual TipoMascota TipoMascota { get; set; }
        //public int TipoMascotaId { get; set; }
        //public virtual CasoMascota CasoMascota { get; set; }
        //public int CasoMascotaId { get; set; }
        public virtual Denuncia Denuncia { get; set; }
        public int DenunciaId { get; set; }
        public virtual ICollection<Foto> Fotos { get; set; }
        public virtual ICollection<ContratoAdopcion> ContratoAdopciones { get; set; }
        //public virtual ICollection<CasoMascota> CasoMascotas { get; set; }
        //public virtual ICollection<DetalleAdopcion> DetalleAdopciones { get; set; }
    }
}
