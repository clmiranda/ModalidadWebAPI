﻿namespace webapi.business.Pagination
{
    public class DenunciaParametros
    {
        private const int MaxPageSize = 100;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public string Busqueda { get; set; } = "";
    }
}
