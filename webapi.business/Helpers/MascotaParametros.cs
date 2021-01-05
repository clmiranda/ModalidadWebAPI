using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Helpers
{
    public class MascotaParametros
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        //public int UserId { get; set; }
        public string Busqueda { get; set; } = "";
        public string Filter { get; set; } = "Adopcion";
        public string OrderBy { get; set; } = "";
        //public MascotaParametros()
        //{
        //    if (Busqueda==null)
        //        Busqueda = "";
        //    if (Filter == null)
        //        Filter = "";
        //}
    }
}