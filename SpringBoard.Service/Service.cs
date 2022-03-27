using SpringBoard.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Service
{
    public class Service<TEntity> : IService<TEntity>
       where TEntity : class
    {
        #region Private Fields
        IUnitOfWork utwk;
        #endregion Private Fields

        #region Constructor
        protected Service(IUnitOfWork utwk)
        {
            this.utwk = utwk;
        }
        #endregion Constructor


        public void Dispose()
        {
            utwk.Dispose();
        }
        public virtual async void Add(TEntity entity)
        {
            ////_repository.Add(entity);
            await utwk.getRepository<TEntity>().Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            //_repository.Update(entity);
             utwk.getRepository<TEntity>().update(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            //   _repository.Delete(entity);
            utwk.getRepository<TEntity>().Delete(entity);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> where)
        {
            // _repository.Delete(where);
            utwk.getRepository<TEntity>().Delete(where);
        }

        public virtual async Task<TEntity> GetById(int id)
        {
            //  return _repository.GetById(id);
            return await utwk.getRepository<TEntity>().GetById(id);
        }

     


        public virtual async Task<IEnumerable<TEntity>> GetMany(Expression<Func<TEntity, bool>> filter = null)
        {
            //  return _repository.GetAll();
            return await utwk.getRepository<TEntity>().getMany(filter);
        }

        public virtual async Task<TEntity> Get(Expression<Func<TEntity, bool>> where)
        {
            //return _repository.Get(where);
            return await utwk.getRepository<TEntity>().Get(where);
        }







        public void Commit()
        {
            try
            {
                utwk.Commit();
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
