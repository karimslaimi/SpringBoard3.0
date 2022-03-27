using SpringBoard.Data.Infrastructure;
using SpringBoard.Domaine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Data.Repositories
{
    public class RepositoryUser:RepositoryBase<Utilisateur>,IRepositoryUser
    {

       

        #region Constructor
        public RepositoryUser(IDatabaseFactory dbFactory):base(dbFactory)
        {
            
        }
        #endregion Constructor



    }
}
