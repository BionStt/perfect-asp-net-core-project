using DAL.Data.Transormers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Data.Extensions
{
    public static class ListExtensions
    {
        public static PaginatedList<T> ToPaginatedList<T>(this IList<T> list, int pageIndex, int pageSize, int total)
        {
            return new PaginatedList<T>(list, pageIndex, pageSize, total);
        }
    }
}
