using System;
using System.Collections.Generic;
using System.Text;

namespace webapi.business.Pagination
{
    public class DenunciaParametros
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
        public string OrderBy { get; set; }
    }
}
