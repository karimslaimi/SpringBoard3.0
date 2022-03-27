using SpringBoard.Data;
using SpringBoard.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
       
        public IRepositoryUser repositoryUser;

        public IRepositoryRapport repositoryRapport;

        public IRepositoryCompteRendu repositoryCompteRendu;



         private DatabContext dataContext;

        IDatabaseFactory dbFactory;
        public UnitOfWork(IDatabaseFactory dbFactory)
        {
            this.dbFactory = dbFactory;
            dataContext = dbFactory.DataContext;
        }


        public IRepositoryUser RepositoryUser => repositoryUser = repositoryUser ?? new RepositoryUser(dbFactory);

        public IRepositoryCompteRendu RepositoryCompteRendu => repositoryCompteRendu = repositoryCompteRendu ?? new RepositoryCompteRendu(dbFactory);

        public IRepositoryRapport RepositoryRapport => repositoryRapport = repositoryRapport ?? new RepositoryRapport(dbFactory);



        public void Commit()
        {
            dataContext.SaveChanges();
        }
      
        public void Dispose()
        {
            dataContext.Dispose();
        }
        public IRepositoryBase<T> getRepository<T>() where T : class
        {
            return new RepositoryBase<T>(dbFactory);
        }
      
    }
}
