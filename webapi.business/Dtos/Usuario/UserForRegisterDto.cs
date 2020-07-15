using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace webapi.business.Dtos.Usuario
{
    public class UserForRegisterDto
    {
        [Required]
        [StringLength(100)]
        public string Email { set; get; }
        [Required]
        public string Username { set; get; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Su Contraseña de tener entre 4 y 8 caracteres!")]
        public string Password { set; get; }
        [Required]
        public string Nombres { get; set; }
        [Required]
        public string Apellidos { get; set; }
        [Required]
        public string Estado { get; set; }
        public string Sexo { get; set; }
    }
}
