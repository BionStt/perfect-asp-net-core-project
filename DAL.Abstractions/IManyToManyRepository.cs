using DAL.Entities.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Abstractions
{
    public interface IManyToManyRepository<TManyToMany> where TManyToMany : class, IManyToMany
    {

        void Create(params TManyToMany[] relation);
        void Delete(params TManyToMany[] relation);
        IQueryable<TManyToMany> Collection { get; }

        IQueryable<TManyToMany> CollectionWithTracking { get; }
        void Update(params TManyToMany[] relations);
        //IQueryable<TManyToMany> Collection { get; }

        //IQueryable<TManyToMany> CollectionWithTracking { get; }
    }
}
