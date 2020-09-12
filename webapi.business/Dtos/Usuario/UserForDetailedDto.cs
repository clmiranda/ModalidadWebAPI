using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Usuario
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Username { set; get; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Domicilio { get; set; }
        public string NumeroCelular { get; set; }
        public string Edad { get; set; }
        public string Sexo { get; set; }

        //Para la foto del Perfil del User y talvez el Created y LastActive
    }
}
