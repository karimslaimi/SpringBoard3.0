using SpringBoard.Data.Infrastructure;
using SpringBoard.Domaine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Data.Repositories
{
    public class RepositoryRapport:RepositoryBase<Rapport>,IRepositoryRapport
    {
        #region Constructor
        public RepositoryRapport(IDatabaseFactory dbFactory) : base(dbFactory)
        {

        }
        #endregion Constructor
    }
}
