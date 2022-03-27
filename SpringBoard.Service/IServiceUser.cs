using Microsoft.AspNetCore.Identity;
using SpringBoard.Domaine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringBoard.Service
{
    public interface IServiceUser
    {
        public Task<Utilisateur> addUserAsync(dynamic user);
        public Task<Utilisateur> updateProfile(dynamic user, UserManager<Utilisateur> userManager);
        public Task<bool> disableAccount(string id);
        public Task<bool> deleteUser(string id);



        public Task<IEnumerable<Consultant>> listConsultant();
        public Task<IEnumerable<Commercial>> listCommercial();
        public Task<IEnumerable<GestionnaireRH>> listGestionnaireRH();
        public Task<IEnumerable<Utilisateur>> Search(string keyword);




    }
}
