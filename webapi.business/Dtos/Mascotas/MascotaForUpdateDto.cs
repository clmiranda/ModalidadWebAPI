using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Mascotas
{
    public class MascotaForUpdateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Especie { get; set; }
        public string Caracteristicas { get; set; }
        public string RasgosParticulares { get; set; }
        public string Tamaño { get; set; }
        public bool? Esterilizado { get; set; }
        public string Edad { get; set; }
        public int? DenunciaId { get; set; }
    }
}
