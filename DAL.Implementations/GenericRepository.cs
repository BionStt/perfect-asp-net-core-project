using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Abstractions;
using DAL.Data.Context;
using DAL.Data.Extensions;
using DAL.Data.Transormers;
using DAL.Entities.Contracts;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public IQueryable<TEntity> Collection => _dbSet.AsNoTracking();

        public IQueryable<TEntity> CollectionWithTracking => _dbSet;

        public async Task<PaginatedList<TEntity>> GetAllAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = FilterQuery(orderBy, predicate, includeProperties);
            var total = await entities.CountAsync();
            entities = entities.Paginate(pageIndex, pageSize);
            var list = await entities.ToListAsync();
            return list.ToPaginatedList(pageIndex, pageSize, total);
        }


        public async Task<PaginatedList<TDTO>> GetAllAsync<TDTO>(
            int pageIndex,
            int pageSize,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IQueryable<TDTO>> selectQuery,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = FilterQuery(orderBy, predicate, includeProperties);
            var selectedEntities = selectQuery(entities);
            var total = await entities.CountAsync();
            entities = entities.Paginate(pageIndex, pageSize);
            var list = await selectedEntities.ToListAsync();
            return list.ToPaginatedList(pageIndex, pageSize, total);
        }


        public void Insert(params TEntity[] items)
        {
            if (items == null)
            {
                throw new NullReferenceException("There are no items to insert");
            }

            foreach (var item in items)
            {
                _dbSet.Add(item);
            }
        }

        public virtual void Delete(params TEntity[] items)
        {
            if (items == null)
            {
                throw new NullReferenceException("There are no items to delete");
            }

            foreach (var item in items)
            {
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    _dbSet.Attach(item);
                }
                _dbSet.Remove(item);
            }
        }

        public void Update(params TEntity[] items)
        {
            if (items == null)
            {
                throw new NullReferenceException("There are no items to update");
            }
            foreach (var item in items)
            {

                _dbSet.Update(item);
            }
        }
        private IQueryable<TEntity> IncludeProperties(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> entities = _dbSet
                    .AsNoTracking()
                ;
            foreach (var includeProperty in includeProperties)
            {
                entities = entities.Include(includeProperty);
            }
            return entities;
        }

        private IQueryable<TEntity> FilterQuery(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>>[] includeProperties)
        {
            var entities = IncludeProperties(includeProperties);
            entities = (predicate != null) ? entities.AsExpandable().Where(predicate) : entities;
            if (orderBy != null)
            {
                entities = orderBy(entities);
            }
            return entities;
        }

    }
}
