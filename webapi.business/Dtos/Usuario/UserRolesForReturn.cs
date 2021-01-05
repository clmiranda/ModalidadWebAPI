﻿using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Dtos.Usuario
{
    public class UserRolesForReturn
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Domicilio { get; set; }
        public string NumeroCelular { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string[] Roles { get; set; }
        public string Edad { get; set; }
        public string Sexo { get; set; }
    }
}