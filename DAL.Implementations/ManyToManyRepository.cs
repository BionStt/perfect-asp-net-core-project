using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Abstractions;
using DAL.Data.Context;
using DAL.Entities.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DAL.Implementations
{
    public class ManyToManyRepository<TManyToMany> : IManyToManyRepository<TManyToMany>
        where TManyToMany : class, IManyToMany
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<TManyToMany> _dbSet;

        public ManyToManyRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<TManyToMany>();
        }

        public void Create(params TManyToMany[] relations)
        {
            foreach (var relation in relations)
            {
                _dbSet.Add(relation);
            }
        }

        public void Update(params TManyToMany[] relations)
        {
            foreach (var relation in relations)
            {
                _dbSet.Update(relation);
            }
        }
        public void Delete(params TManyToMany[] relations)
        {
            foreach (var relation in relations)
            {
                _dbSet.Remove(relation);
            }
        }

        public IQueryable<TManyToMany> Collection => _dbSet.AsNoTracking();

        public IQueryable<TManyToMany> CollectionWithTracking => _dbSet;

    }
}
