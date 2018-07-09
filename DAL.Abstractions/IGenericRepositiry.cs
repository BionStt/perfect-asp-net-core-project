
using DAL.Data.Transormers;
using DAL.Entities.Contracts;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    { 
        IQueryable<TEntity> Collection { get; }

        IQueryable<TEntity> CollectionWithTracking { get; }

        Task<PaginatedList<TEntity>> GetAllAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties);


        Task<PaginatedList<TDTO>> GetAllAsync<TDTO>(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TDTO>> selectQuery,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties);

        void Insert(params TEntity[] items);

        void Delete(params TEntity[] items);

        void Update(params TEntity[] items);
    }
}
