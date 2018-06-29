using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsApp
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        IQueryable<TEntity> Get();
        TEntity Get(string id);
        void Delete(TEntity entity);
        Task<bool> SaveChangesAsync();
    }
}
