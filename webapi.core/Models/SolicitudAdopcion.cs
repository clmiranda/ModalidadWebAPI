﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace webapi.core.Models
{
    public partial class SolicitudAdopcion : BaseEntity
    {
        [Required]
        [MaxLength(300)]
        public string NombreCompleto { get; set; }
        [Required]
        [MaxLength(200)]
        public string Domicilio { get; set; }
        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaSolicitudAdopcion { get; set; }
        public DateTime FechaAdopcion { get; set; }
        [Required]
        [MaxLength(20)]
        public string Estado { get; set; }
        public virtual Mascota Mascota { get; set; }
        public int? MascotaId { get; set; }
        public virtual Seguimiento Seguimiento { get; set; }
        public virtual ContratoAdopcion ContratoAdopcion { get; set; }
        public virtual AdopcionRechazada AdopcionRechazada { get; set; }
        public virtual AdopcionCancelada AdopcionCancelada { get; set; }
    }
}