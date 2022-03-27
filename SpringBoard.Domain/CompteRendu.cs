using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Domaine
{
    public class CompteRendu
    {
        [Key]
        public int id { get; set; }

        public bool statut { get; set; }

        public DateTime validation { get; set; }

        public DateTime date { get; set; }

        public ICollection<Rapport> Rapports { get; set; }


        public virtual Consultant Consultant { get; set; }
    }
}
