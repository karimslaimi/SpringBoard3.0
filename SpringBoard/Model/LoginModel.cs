using System.ComponentModel.DataAnnotations;

namespace SpringBoard.API.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
