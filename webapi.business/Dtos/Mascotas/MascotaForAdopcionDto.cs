using webapi.business.Dtos.Adopciones;
using webapi.business.Dtos.Fotos;

namespace webapi.business.Dtos.Mascotas
{
    public class MascotaForAdopcionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Estado { get; set; }
        public virtual FotoForReturnDto Foto { get; set; }
        public virtual SolicitudAdopcionReturnDto SolicitudAdopcion { get; set; }
    }
}
