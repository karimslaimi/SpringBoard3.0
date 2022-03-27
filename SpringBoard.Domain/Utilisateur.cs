
using Microsoft.AspNetCore.Identity;

namespace SpringBoard.Domaine
{
    public class Utilisateur : IdentityUser
    {
        public string Firstname {get; set; }
        public string LastName { get; set; }


    }
}
