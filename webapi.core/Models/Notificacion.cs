using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.core.Models
{
    public partial class Notificacion : BaseEntity
    {
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
    }
}
