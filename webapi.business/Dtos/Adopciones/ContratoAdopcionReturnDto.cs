﻿using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.ContratoRechazo;
using webapi.business.Dtos.Mascotas;
using webapi.core.Models;

namespace webapi.business.Dtos.Adopciones
{
    public class ContratoAdopcionReturnDto
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Ci { get; set; }
        public string Domicilio { get; set; }
        public string NumeroCelular { get; set; }
        //public string NumeroCelular { get; set; }
        public string Pregunta1 { get; set; }
        public string Pregunta2 { get; set; }
        public string Pregunta3 { get; set; }
        public string Pregunta4 { get; set; }
        public string Pregunta5 { get; set; }
        public string Pregunta6 { get; set; }
        public string Pregunta7 { get; set; }
        //public string Edad { get; set; }
        //public bool TerminosCondiciones { get; set; }
        public DateTime FechaSolicitudAdopcion { get; set; }
        public DateTime FechaAdopcion { get; set; }
        public string Estado { get; set; }
        public MascotaForReturn Mascota { get; set; }
        public ContratoRechazoForReturnDto ContratoRechazo { get; set; }
        //public int UserId { get; set; }
        //public ContratoAdopcionDto()
        //{
        //    FechaAdopcion = DateTime.Now;
        //}
    }
}
