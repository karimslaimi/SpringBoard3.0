using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Domaine
{
    public class Rapport
    {
        [Key]
        public int id { get; set; }

        public DateTime date { get; set; }

        public double valeur { get; set; }

        public virtual CompteRendu CompteRendu { get; set; }


    
    
    
    }
}
