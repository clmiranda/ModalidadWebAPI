using webapi.business.Dtos.Persona;

namespace webapi.business.Dtos.Usuario
{
    public class UserRolesForReturn
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public string Estado { get; set; }
        public virtual PersonaForReturnDto Persona { get; set; }
    }
}
