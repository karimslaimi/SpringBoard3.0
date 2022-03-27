using SpringBoard.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Data.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryUser RepositoryUser { get; }

        IRepositoryCompteRendu RepositoryCompteRendu { get; }

        IRepositoryRapport RepositoryRapport { get; }



        IRepositoryBase<T> getRepository<T>() where T : class; 
        void Commit();
       
    }

}
