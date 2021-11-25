using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public partial class User: IdentityUser<int>
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Domicilio { get; set; }
        public string NumeroCelular { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
        public string Sexo { get; set; }
        public virtual ICollection<Seguimiento> Seguimientos { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}