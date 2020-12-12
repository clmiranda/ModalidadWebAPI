using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace webapi.business.Dtos.Usuario
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        //public string ConfirmPassword { get; set; }
        //[Required]
        //public string Token { get; set; }
    }
}
