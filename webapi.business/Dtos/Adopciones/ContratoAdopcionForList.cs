using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Mascotas;
using webapi.business.Dtos.Usuario;

namespace webapi.business.Dtos.Adopciones
{
    public class ContratoAdopcionForList
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Domicilio { get; set; }
        public string RazonAdopcion { get; set; }
        public DateTime FechaSolicitudAdopcion { get; set; }
        public DateTime FechaAdopcion { get; set; }
        public string Estado { get; set; }
        public virtual MascotaForReturn Mascota { get; set; }
        public int MascotaId { get; set; }

    }
}
