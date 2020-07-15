using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Usuario
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string Username { set; get; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Sexo { get; set; }
    }
}
