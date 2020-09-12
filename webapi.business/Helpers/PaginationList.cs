﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webapi.business.Helpers
{
    public class PaginationList<T> : List<T>
	{
		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }
		public bool HasPrevious => CurrentPage > 1;
		public bool HasNext => CurrentPage < TotalPages;
		public PaginationList(List<T> items, int count, int pageNumber, int pageSize)
		{
			TotalCount = count;
			PageSize = pageSize;
			CurrentPage = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
			AddRange(items);
		}
		public static async Task<PaginationList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var count = await source.CountAsync();
			var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
			return new PaginationList<T>(items, count, pageNumber, pageSize);
		}
	}
}
