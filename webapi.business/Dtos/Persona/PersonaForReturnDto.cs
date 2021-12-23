using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Persona
{
    public class PersonaForReturnDto
    {
        public int Id { get; set; }
        //public string Username { set; get; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Domicilio { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Edad { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
        //public string Email { get; set; }
    }
}
