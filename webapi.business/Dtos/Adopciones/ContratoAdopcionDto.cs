using Microsoft.AspNetCore.Http;

namespace webapi.business.Dtos.Adopciones
{
    public class ContratoAdopcionDto
    {
        public int Id { get; set; }
        public string IdPublico { get; set; }
        public string Url { get; set; }
        public IFormFile Archivo { get; set; }
    }
}
