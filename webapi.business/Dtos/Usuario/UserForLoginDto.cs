using System.ComponentModel.DataAnnotations;

namespace webapi.business.Dtos.Usuario
{
    public class UserForLoginDto
    {
        [Required(ErrorMessage = "El campo Usuario o Correo Electrónico es obligatorio")]
        public string UsernameEmail { get; set; }
        [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        public string Password { get; set; }
    }
}