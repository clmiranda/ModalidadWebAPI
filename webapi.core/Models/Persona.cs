using System;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public class Persona: BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Nombres { get; set; }
        [Required]
        [MaxLength(200)]
        public string Apellidos { get; set; }
        [Required]
        [MaxLength(200)]
        public string Domicilio { get; set; }
        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [Required]
        [MaxLength(20)]
        public string Genero { get; set; }
        [Required]
        public virtual User User { get; set; }
    }
}