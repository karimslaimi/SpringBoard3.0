using Microsoft.AspNetCore.Identity;
using SpringBoard.Data.Infrastructure;
using SpringBoard.Domaine;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;

namespace SpringBoard.Service
{
    public class ServiceUser : IServiceUser
    {
        static IDatabaseFactory dbf = null;
        static IUnitOfWork utwk = null;
        private UserManager<Utilisateur> userManager;
        private IServiceMail ServiceMail;


        public ServiceUser(UserManager<Utilisateur> _userManager,IServiceMail _serviceMail)
        {
            this.userManager = _userManager;
            dbf = new DatabaseFactory();
            utwk = new UnitOfWork(dbf);
            this.ServiceMail = _serviceMail;
        }





        public async Task<Utilisateur> addUserAsync(dynamic us)
        {
            string role = us.role;


            if (string.IsNullOrWhiteSpace(role) || string.IsNullOrWhiteSpace(us.password) || string.IsNullOrEmpty(us.email))
            {
                return null;
            }

            if (role == "RH")
            {
                GestionnaireRH user = new GestionnaireRH();
                user.Email = us.email;
                user.LastName = us.lastName;
                user.Firstname = us.firstName;
                user.UserName = us.email;

                var result = await userManager.CreateAsync(user, us.password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "RH");
                }
                else
                {
                    return null;
                }
                return user;

            }



            else if (role == "Commercial")
            {
                Commercial user = new Commercial();
                user.Email = us.email;
                user.LastName = us.lastName;
                user.Firstname = us.firstName;
                user.UserName = us.email;
                var result = await userManager.CreateAsync(user, us.password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Commercial");
                }
                else
                {
                    return null;
                }
                return user;
            }
            else if (role == "Administrateur")
            {
                Administrateur user = new Administrateur();
                user.Email = us.email;
                user.LastName = us.lastName;
                user.Firstname = us.firstName;
                user.UserName = us.email;

                var result = await userManager.CreateAsync(user, us.password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrateur");
                }
                else
                {
                    return null;
                }
                return user;
            }
            else
            {
                return null;
            }

        }


        public async Task<Utilisateur> updateProfile(dynamic user)
        {
            Utilisateur us = await userManager.FindByIdAsync(user.id);

            if (us == null)
            {
                return null;
            }

            us.Firstname = user.firstName;
            us.LastName = user.lastName;
            us.Email = user.email;
            us.UserName = us.Email;


            if (!string.IsNullOrEmpty(user.password) && !string.IsNullOrWhiteSpace(user.password))
            {
                await userManager.RemovePasswordAsync(us);
                await userManager.AddPasswordAsync(us, user.password);
            }

            await userManager.UpdateAsync(us);
            return us;
        }


        public async Task<IEnumerable<Commercial>> listCommercial(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return await utwk.getRepository<Commercial>().getMany(x => x.Firstname.Contains(keyword) || x.LastName.Contains(keyword) || x.UserName.Contains(keyword));

            }
            return await utwk.getRepository<Commercial>().getAll();



        }

        public async Task<IEnumerable<Consultant>> listConsultant(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return await utwk.getRepository<Consultant>().getMany(x => x.Firstname.Contains(keyword) ||
                x.LastName.Contains(keyword) || x.UserName.Contains(keyword) || x.competence.Contains(keyword));

            }
            return await utwk.getRepository<Consultant>().getAll();
        }

        public async Task<IEnumerable<GestionnaireRH>> listGestionnaireRH(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return await utwk.getRepository<GestionnaireRH>().getMany(x => x.Firstname.Contains(keyword) || x.LastName.Contains(keyword) || x.UserName.Contains(keyword));

            }
            return await utwk.getRepository<GestionnaireRH>().getAll();
        }

        public async Task<IEnumerable<Utilisateur>> Search(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return await utwk.RepositoryUser.getMany(x => x.Firstname.Contains(keyword) || x.LastName.Contains(keyword) || x.UserName.Contains(keyword));

            }
            return await utwk.RepositoryUser.getAll();
        }


        public async Task<bool> deleteUser(string id)
        {
            Utilisateur user = await userManager.FindByIdAsync(id);

            //get the user and all of his login and roles
            var logins = await userManager.GetLoginsAsync(user);
            var rolesForUser = await userManager.GetRolesAsync(user);


            IdentityResult result = IdentityResult.Success;

            //delete all the login history
            foreach (var login in logins)
            {
                result = await userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                if (result != IdentityResult.Success)
                    break;
            }

            //delete all roles
            if (result == IdentityResult.Success)
            {
                foreach (var item in rolesForUser)
                {
                    result = await userManager.RemoveFromRoleAsync(user, item);
                    if (result != IdentityResult.Success)
                        break;
                }
            }

            //delete the user
            if (result == IdentityResult.Success)
            {
                try
                {
                    result = await userManager.DeleteAsync(user);

                    if (result == IdentityResult.Success)
                    {

                        utwk.Commit();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    await disableAccount(id);//disable the account otherwise
                    return false;
                }

            }


            return false;




        }


        public async Task<bool> disableAccount(string id)
        {
            //if the user can't be deleted then disable his account
            //fetch the user by id 

            Utilisateur user = await userManager.FindByIdAsync(id);

            //if there is no user with the specific id return false
            if (user == null)
            {
                return false;
            }

            //create a date late enough
            var lockoutEndDate = new DateTime(2999, 01, 01);
            //lockout the account
            var lockuser = await userManager.SetLockoutEnabledAsync(user, true);
            var lockdate = await userManager.SetLockoutEndDateAsync(user, lockoutEndDate);

            return lockdate.Succeeded && lockuser.Succeeded;


        }

        public async Task<Consultant> addConsultant(dynamic us)
        {
            string commid = us.commid;

            Commercial commercial = await utwk.getRepository<Commercial>().Get(x => x.Id == commid);
            if (commercial == null)
            {
                return null;
            }

            Consultant user = new Consultant();
            user.Email = us.email;
            user.LastName = us.lastName;
            user.Firstname = us.firstName;
            user.UserName = us.email;
            user.commid = us.commid;
            user.competence = us.competence;
            var result = await userManager.CreateAsync(user, us.password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Consultant");
            }
            else
            {
                return null;
            }
            return user;

        }

        public async Task<Consultant> updateConsultant(dynamic user, string userid)
        {


            Consultant us = (Consultant)await userManager.FindByIdAsync(userid);

            if (us == null)
            {
                return null;
            }

            us.Firstname = user.firstName;
            us.LastName = user.lastName;
            us.Email = user.email;
            us.competence = user.competence;

            if (!string.IsNullOrEmpty(user.password) && !string.IsNullOrWhiteSpace(user.password))
            {
                await userManager.RemovePasswordAsync(us);
                await userManager.AddPasswordAsync(us, user.password);
            }
            await userManager.UpdateAsync(us);
            return us;
        }





        public async Task<bool> forgotPassword(string email)
        {
            

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return false;


            var token = await userManager.GeneratePasswordResetTokenAsync(user);
        
            string emailResponse = ServiceMail.sendMail(user.Email,"reset token", token);

            return emailResponse != null;
            
        }

        public async Task<string> resetPassword(string email, string token, string password)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
               return "user not found";


            var resetPassResult = await userManager.ResetPasswordAsync(user, token, password);

            if (!resetPassResult.Succeeded)
            {
                return resetPassResult.Errors.ToString();
              
            }

            return "password changed";
        }


    }
}
