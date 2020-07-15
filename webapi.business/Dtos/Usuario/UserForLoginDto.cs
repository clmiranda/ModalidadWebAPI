using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace webapi.business.Dtos.Usuario
{
    public class UserForLoginDto
    {
        [Required(ErrorMessage = "Es campo de Usuario es requerido")]
        public string Username { get; set; }
        [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
