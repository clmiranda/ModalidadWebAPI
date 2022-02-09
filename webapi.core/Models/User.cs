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
        public DateTime FechaCreacion { get; set; }
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; }
        public virtual List<Seguimiento> Seguimientos { get; set; }
        public virtual List<UserRole> UserRoles { get; set; }
        public virtual Persona Persona { get; set; }
    }
}