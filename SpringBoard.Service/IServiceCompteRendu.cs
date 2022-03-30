using SpringBoard.Domaine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Service
{
    public interface IServiceCompteRendu
    {
        public Task<Rapport> addRapportToCR(DateTime date, double value,string userId);
        public Task<CompteRendu> GetCRbyDateAndUser(DateTime date, string userId);
        public Task<IEnumerable<CompteRendu>> getUserCR(string userID);
        public Task<IEnumerable<CompteRendu>> getCRbyDate(DateTime date);
        public Task<CompteRendu> validateCR(int crid);
        public Task<CompteRendu> unlockCR(int crid);
        public Task<bool> delete(int cr);
        public Task<IEnumerable<CompteRendu>> getCRByCommercial(string userid);


    }
}
