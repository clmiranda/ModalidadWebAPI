using System;
using webapi.business.Dtos.SolicitudAdopcionRechazada;
using webapi.business.Dtos.Mascotas;
using webapi.business.Dtos.SolicitudAdopcionCancelada;

namespace webapi.business.Dtos.Adopciones
{
    public class SolicitudAdopcionForList
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaSolicitudAdopcion { get; set; }
        public DateTime FechaAdopcion { get; set; }
        public string Estado { get; set; }
        public virtual SolicitudAdopcionRechazadaForReturnDto AdopcionRechazada { get; set; }
        public virtual SolicitudAdopcionCanceladaForReturnDto AdopcionCancelada { get; set; }
        public virtual MascotaForReturn Mascota { get; set; }
        public virtual ContratoAdopcionForReturnDto ContratoAdopcion { get; set; }
        public int MascotaId { get; set; }
        public int NumeroAntecedentes { get; set; }

    }
}
