using System;
using webapi.business.Dtos.Persona;

namespace webapi.business.Dtos.Usuario
{
    public class UserRolesForReturn
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        //public string Nombres { get; set; }
        //public string Apellidos { get; set; }
        public string Email { get; set; }
        //public string Domicilio { get; set; }
        //public string NumeroCelular { get; set; }
        //public DateTime FechaNacimiento { get; set; }
        public string[] Roles { get; set; }
        //public string Edad { get; set; }
        //public string Sexo { get; set; }
        public string Estado { get; set; }
        public virtual PersonaForReturnDto Persona { get; set; }
    }
}
