using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Fotos;

namespace webapi.business.Dtos.Mascotas
{
    public class MascotaForDetailedDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Tamaño { get; set; }
        public bool? Esterilizado { get; set; }
        public string Edad { get; set; }
        public DateTime FechaAgregado { get; set; }
        public string EstadoSituacion { get; set; }
        //public byte[] Imagen { get; set; }
        //public int IdTipoMascota { get; set; }
        public int CasoMascotaId { get; set; }
        public string FotoUrl { get; set; }
        public string TituloDenuncia { get; set; }
        public string TituloCaso { get; set; }
        public virtual ICollection<FotoForReturnDto> Fotos { get; set; }
    }
}
