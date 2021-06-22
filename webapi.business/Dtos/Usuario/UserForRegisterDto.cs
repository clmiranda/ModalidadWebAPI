using System;

namespace webapi.business.Dtos.Usuario
{
    public class UserForRegisterDto
    {
        public string Email { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Domicilio { get; set; }
        public string NumeroCelular { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get;  }
        public UserForRegisterDto()
        {
            FechaCreacion = DateTime.Now;
            Estado = "Activo";
        }
    }
}
