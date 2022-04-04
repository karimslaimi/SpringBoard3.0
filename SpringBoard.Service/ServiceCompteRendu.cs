using Microsoft.Extensions.Configuration;
using SpringBoard.Data.Infrastructure;
using SpringBoard.Domaine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Service
{
    public class ServiceCompteRendu : IServiceCompteRendu
    {
        static IDatabaseFactory dbf = null;
        static IUnitOfWork utwk = null;
        private IServiceMail ServiceMail;

        public ServiceCompteRendu( IServiceMail _serviceMail)
        {
            dbf = new DatabaseFactory();
            utwk = new UnitOfWork(dbf);
            this.ServiceMail = _serviceMail;
        }
        //TODO create a mapper for all entities

        public async Task<Rapport> addRapportToCR(DateTime date, double value, string userId)
        {

            CompteRendu cr = await utwk.RepositoryCompteRendu.Get(x => x.date.Month == date.Month && x.Consultant.Id == userId);

            if (cr == null)
            {
                cr = new CompteRendu();
                cr.Consultant = await utwk.getRepository<Consultant>().Get(x => x.Id == userId);
                cr.date = DateTime.UtcNow;
                cr.statut = false;
                await utwk.RepositoryCompteRendu.Add(cr);
                utwk.Commit();

            }


            Rapport rapport = new Rapport();
            rapport.CompteRendu = cr;
            rapport.date = date;
            rapport.valeur = value;
            await utwk.RepositoryRapport.Add(rapport);
            utwk.Commit();



            return rapport;
        }

        public async Task<bool> delete(int crid)
        {
            CompteRendu cr = await utwk.RepositoryCompteRendu.GetById(crid);
            if (cr != null)
            {
                await utwk.RepositoryRapport.Delete(x => x.CompteRendu == cr);
                await utwk.RepositoryCompteRendu.Delete(x => x.id == crid);
                utwk.Commit();
                return true;
            }
            else
            {
                return false;
            }

        }


        public async Task<CompteRendu> GetCRbyDateAndUser(DateTime date, string userId)
        {
            return  await utwk.RepositoryCompteRendu.Get(x => x.date.Month == date.Month && x.Consultant.Id == userId, "Consultant,Rapports");


        }

        public async Task<IEnumerable<CompteRendu>> getCRbyDate(DateTime date)
        {
            IEnumerable<CompteRendu> compteRendus = await utwk.RepositoryCompteRendu.getMany(x => x.date.Month == date.Month,"Consultant,Rapports");

         

            return compteRendus;
        }

        public async Task<IEnumerable<CompteRendu>> getUserCR(string userID)
        {
            IEnumerable<CompteRendu> compteRendus = await utwk.RepositoryCompteRendu.getMany(x => x.Consultant.Id == userID, "Consultant,Rapports");

         

            return compteRendus;
        }

        public async Task<CompteRendu> unlockCR(int crid)
        {
            CompteRendu cr = await utwk.RepositoryCompteRendu.GetById(crid);
            if (cr == null)
            {
                return null;
            }
            cr.statut = false;
            utwk.RepositoryCompteRendu.update(cr);
            utwk.Commit();
            return cr;


        }

        public async Task<CompteRendu> validateCR(int crid)
        {
            CompteRendu cr = await utwk.RepositoryCompteRendu.GetById(crid);
            if (cr == null)
            {
                return null;
            }

            cr.statut = true;
            cr.validation = DateTime.UtcNow; ;
            utwk.RepositoryCompteRendu.update(cr);
            utwk.Commit();
            ServiceMail.sendMail("Votre compte rendu créé le " + cr.date + " a été validé", "Compte rendu validé", cr.Consultant.Email);
            return cr;

        }

       

        public async Task<IEnumerable<CompteRendu>> getCRByCommercial(string userid)
        {
            IEnumerable<CompteRendu> compteRendus = await utwk.RepositoryCompteRendu.getMany(x => x.Consultant.commid == userid, "Consultant,Rapports");

          

            return compteRendus;
        }
    }
}
