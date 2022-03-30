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
        public Task<Consultant> addConsultant(dynamic user);
        public Task<Consultant> updateConsultant(dynamic user, string userid);
        public Task<Utilisateur> updateProfile(dynamic user);
        public Task<bool> disableAccount(string id);
        public Task<bool> deleteUser(string id);



        public Task<IEnumerable<Consultant>> listConsultant(string keyword);
        public Task<IEnumerable<Commercial>> listCommercial(string keyword);
        public Task<IEnumerable<GestionnaireRH>> listGestionnaireRH(string keyword);
        public Task<IEnumerable<Utilisateur>> Search(string keyword);

        public Task<bool> forgotPassword(string email);
        public Task<string> resetPassword(string email, string token, string password);




    }
}
