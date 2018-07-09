using DAL.Data.Transormers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace DAL.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIndex, int pageSize, int total)
        {
            var list = query.ToList();
            return new PaginatedList<T>(list, pageIndex, pageSize, total);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            if (pageIndex <= 0) pageIndex = 1;
            var entities = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return entities;
        }


        
    }

}
