using System.ComponentModel.DataAnnotations;

namespace webapi.business.Dtos.Usuario
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Ingrese un Email válido")]
        public string Email { get; set; }
    }
}
