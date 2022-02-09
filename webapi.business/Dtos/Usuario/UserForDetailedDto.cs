using System;
using webapi.business.Dtos.Persona;

namespace webapi.business.Dtos.Usuario
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Email { get; set; }
        public virtual PersonaForReturnDto Persona { get; set; }
    }
}
