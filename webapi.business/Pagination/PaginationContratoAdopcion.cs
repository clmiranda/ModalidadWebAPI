using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Adopciones;

namespace webapi.business.Pagination
{
    public class PaginationContratoAdopcion
    {
		public IEnumerable<ContratoAdopcionReturnDto> Items { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }
		public bool HasPrevious => CurrentPage > 1;
		public bool HasNext => CurrentPage < TotalPages;
	}
}
