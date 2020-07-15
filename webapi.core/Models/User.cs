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
        public string Estado { get; set; }
        public string Sexo { get; set; }
        //public virtual ICollection<DetalleAdopcion> DetalleAdopciones { get; set; }
        public virtual ICollection<Foto> Fotos { get; set; }
        public virtual ICollection<ContratoAdopcion> ContratoAdopciones { get; set; }
        public virtual ICollection<ReporteSeguimiento> ReporteSeguimientos { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
