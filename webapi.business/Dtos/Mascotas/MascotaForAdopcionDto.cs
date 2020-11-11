using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.Fotos;
using webapi.core.Models;

namespace webapi.business.Dtos.Mascotas
{
    public class MascotaForAdopcionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string EstadoSituacion { get; set; }
        public virtual FotoForReturnDto Foto { get; set; }
        public virtual ContratoAdopcionReturnDto ContratoAdopcion { get; set; }
    }
}
