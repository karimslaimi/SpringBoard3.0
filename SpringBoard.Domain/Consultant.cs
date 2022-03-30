using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SpringBoard.Domaine
{
    public class Consultant:Utilisateur
    {
        public string commid { get; set; }
        public string competence { get; set; }

        [JsonIgnore]
        public virtual List<CompteRendu> CompteRendus { get; set; }
    }
}
