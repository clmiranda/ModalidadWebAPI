using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public partial class User: IdentityUser<int>
    {
        public User()
        {
            Seguimientos = new List<Seguimiento>();
            UserRoles = new List<UserRole>();
        }
        //public string Nombres { get; set; }
        //public string Apellidos { get; set; }
        //public string Domicilio { get; set; }
        //public string NumeroCelular { get; set; }
        //public DateTime FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; }
        //public string Sexo { get; set; }
        public virtual List<Seguimiento> Seguimientos { get; set; }
        public virtual List<UserRole> UserRoles { get; set; }
        public virtual Persona Persona { get; set; }
        //public virtual int PersonaId { get; set; }
    }
}