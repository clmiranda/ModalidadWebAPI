using System;
using webapi.business.Dtos.Persona;

namespace webapi.business.Dtos.Usuario
{
    public class UserForRegisterDto
    {
        public string Email { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get;  }
        public virtual PersonaForCreateUpdateDto Persona { get; set; }
        public UserForRegisterDto()
        {
            FechaCreacion = DateTime.Now;
            Estado = "Activo";
        }
    }
}
