using DAL.Abstractions;
using DAL.Data.Context;
using DAL.Entities.Contracts;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace DAL.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private Hashtable _repositories;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }


        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }
            var type = typeof(TEntity);
            var typeName = type.Name;

            if (!_repositories.ContainsKey(typeName))
            {
                var repositoryType = typeof(GenericRepository<>);
                _repositories.Add(typeName, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context));
            }

            return (IGenericRepository<TEntity>)_repositories[typeName];
        }
        public IManyToManyRepository<TManyToMany> ManyToManyRepository<TManyToMany>() where TManyToMany : class, IManyToMany
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }
            var type = typeof(TManyToMany);
            var typeName = type.Name;

            if (!_repositories.ContainsKey(typeName))
            {
                var repositoryType = typeof(ManyToManyRepository<>);
                _repositories.Add(typeName, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TManyToMany)), _context));
            }

            return (IManyToManyRepository<TManyToMany>)_repositories[typeName];
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
