using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace webapi.business.Dtos.Usuario
{
    public class UserForRegisterDto
    {
        //[Required]
        //[StringLength(100)]
        public string Email { set; get; }
        //[Required]
        public string UserName { set; get; }
        //[Required]
        //[StringLength(8, MinimumLength = 4, ErrorMessage = "Su Contraseña de tener entre 4 y 8 caracteres!")]
        public string Password { set; get; }
        //[Required]
        public string Nombres { get; set; }
        //[Required]
        public string Apellidos { get; set; }
        //[Required]
        public string Domicilio { get; set; }
        //[Required]
        public string NumeroCelular { get; set; }
        //[Required]
        public string Sexo { get; set; }
        //[Required]
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
