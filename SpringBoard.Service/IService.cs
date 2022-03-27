using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpringBoard.Service
{

    public interface IService<T> : IDisposable
    where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        Task<T> GetById(int id);


        Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> where = null);
        Task<T> Get(Expression<Func<T, bool>> where);




        void Commit();

    }
}