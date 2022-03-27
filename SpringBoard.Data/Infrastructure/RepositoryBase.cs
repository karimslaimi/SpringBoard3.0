using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SpringBoard.Data.Infrastructure
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private DatabContext dataContext;
        private readonly DbSet<T> dbset;
        IDatabaseFactory databaseFactory;

 



        public RepositoryBase(IDatabaseFactory dbFactory)
        {
            this.databaseFactory = dbFactory;
            dbset = DataContext.Set<T>();


        }
        protected DatabContext DataContext
        {
            get { return dataContext = databaseFactory.DataContext; }
        }


        public virtual async Task<T> Get(Expression<Func<T, bool>> where)
        {
            return await dbset.Where(where).FirstOrDefaultAsync();
        }

        public virtual async Task<IEnumerable<T>> getAll()
        {
            return await dbset.ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await dbset.FindAsync(id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbset.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = await dbset.Where<T>(where).ToListAsync();
            foreach (T obj in objects)
                dbset.Remove(obj);
            return true;
        }

        public virtual bool Delete(T entity)
        {
            dbset.Remove(entity);
            return true;
        }

        public virtual T update(T entity)
        {
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual async Task<IEnumerable<T>> getMany(Expression<Func<T, bool>> predicate)
        {
            return await dbset.Where(predicate).ToListAsync();
        }
    }
}
