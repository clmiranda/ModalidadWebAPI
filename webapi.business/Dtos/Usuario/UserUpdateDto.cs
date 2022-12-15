using webapi.business.Dtos.Persona;

namespace webapi.business.Dtos.Usuario
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public virtual PersonaForCreateUpdateDto Persona { get; set; }
    }
}
