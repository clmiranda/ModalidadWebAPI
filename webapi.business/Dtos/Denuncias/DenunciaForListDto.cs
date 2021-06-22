using webapi.business.Dtos.Mascotas;

namespace webapi.business.Dtos.Denuncias
{
    public class DenunciaForListDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public virtual MascotaForDetailedDto Mascota { get; set; }
        public int MascotaId { get; set; }
    }
}
