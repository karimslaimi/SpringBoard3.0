using SpringBoard.Domaine;
using System;

namespace SpringBoard.Model
{
    public class ReportModel
    {
        public int id { get; set; }

        public DateTime date { get; set; }

        public double valeur { get; set; }

        public int crID { get; set; }



       public ReportModel Mapper(Rapport rapport)
        {
            this.id = rapport.id;
            this.date = rapport.date;
            this.valeur = rapport.valeur;
            this.crID = rapport.CompteRendu.id!=0?rapport.CompteRendu.id:0;

            return this;

        }
    }
}
