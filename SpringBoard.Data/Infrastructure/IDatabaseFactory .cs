using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        DatabContext DataContext { get; }
    }

}
