using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.Denuncias;
using webapi.business.Dtos.Fotos;
using webapi.business.Dtos.ReporteTratamientos;
using webapi.core.Models;

namespace webapi.business.Dtos.Mascotas
{
    public class MascotaForDetailedDto
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
        public DateTime FechaCreacion { get; set; }
        public List<ReporteTratamientoForReturnDto> ReporteTratamientos { get; set; }
        public string Estado { get; set; }
        public virtual DenunciaForDetailedDto Denuncia { get; set; }
        public int DenunciaId { get; set; }
        public virtual ICollection<FotoForReturnDto> Fotos { get; set; }
        public virtual ContratoAdopcionReturnDto ContratoAdopcion { get; set; }
    }
}
