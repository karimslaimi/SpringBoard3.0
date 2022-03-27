using SpringBoard.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpringBoard.Domaine;

namespace SpringBoard.Data.Repositories
{
    public class RepositoryCompteRendu:RepositoryBase<CompteRendu>,IRepositoryCompteRendu
    {
        #region Constructor
        public RepositoryCompteRendu(IDatabaseFactory dbFactory) : base(dbFactory)
        {

        }
        #endregion Constructor
    }
}
