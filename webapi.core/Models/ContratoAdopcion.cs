namespace webapi.core.Models
{
    public class ContratoAdopcion: BaseEntity
    {
        public string Url { get; set; }
        public string IdPublico { get; set; }
        public virtual SolicitudAdopcion SolicitudAdopcion { get; set; }
        public int? SolicitudAdopcionId { get; set; }
    }
}
