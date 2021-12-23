using System;

namespace webapi.business.Dtos.Mascotas
{
    public class MascotaForCreateDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Sexo { get; set; }
        public string Especie { get; set; }
        public string Caracteristicas { get; set; }
        public string RasgosParticulares { get; set; }
        public string Tamano { get; set; }
        public bool? Esterilizado { get; set; }
        public string Edad { get; set; }
        public DateTime FechaCreacion { get; }
        public string Estado { get; }
        public int? DenunciaId { get; set; }
        public MascotaForCreateDto()
        {
            FechaCreacion = DateTime.Now;
            Estado = "Inactivo";
        }
    }
}
