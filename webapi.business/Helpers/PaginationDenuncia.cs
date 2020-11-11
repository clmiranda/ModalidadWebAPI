using System;
using System.Collections.Generic;
using System.Text;
using webapi.business.Dtos.Denuncias;
using webapi.core.Models;

namespace webapi.business.Helpers
{
    public class PaginationDenuncia
    {
		public IEnumerable<DenunciaForListDto> Items { get; set; }
		public int CurrentPage { get;  set; }
		public int TotalPages { get;  set; }
		public int PageSize { get;  set; }
		public int TotalCount { get;  set; }
		public bool HasPrevious => CurrentPage > 1;
		public bool HasNext => CurrentPage < TotalPages;
		//public PaginationDenuncia(/*List<T> items, */int count, int pageNumber, int pageSize)
		//{
		//	TotalCount = count;
		//	PageSize = pageSize;
		//	CurrentPage = pageNumber;
		//	TotalPages = (int)Math.Ceiling(count / (double)pageSize);
		//	//AddRange(items);
		//}
		//public static async Task<PaginationList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
		//{
		//	var count = await source.CountAsync();
		//	var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
		//	return new PaginationList<T>(items, count, pageNumber, pageSize);
		//}
	}
}
