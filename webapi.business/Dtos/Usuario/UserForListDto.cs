using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Seguimientos;

namespace webapi.business.Dtos.Usuario
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Sexo { get; set; }
        public virtual ICollection<SeguimientoForReturnDto> Seguimientos { get; set; }
    }
}
