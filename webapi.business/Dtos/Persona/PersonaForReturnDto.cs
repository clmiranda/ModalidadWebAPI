using System;

namespace webapi.business.Dtos.Persona
{
    public class PersonaForReturnDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Domicilio { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Edad { get; set; }
        public string Genero { get; set; }
        public string Telefono { get; set; }
    }
}
