using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Data.Infrastructure
{
    public interface IRepositoryBase<T>
     where T : class
    {
        



        Task<T> Get(Expression<Func<T, bool>> where);

        Task<IEnumerable<T>> getAll();
        Task<T> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Delete(Expression<Func<T, bool>> where);
        bool Delete(T entity);
        T update(T entity);
        Task<IEnumerable<T>> getMany(Expression<Func<T, bool>> predicate);


    }
}
