using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpringBoard.API.Model;
using SpringBoard.Domaine;
using SpringBoard.Model;
using SpringBoard.Service;
using System.Text.Json;
using System.Threading.Tasks;

namespace SpringBoard.API.Controllers
{
   
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IServiceUser serviceUser;
        private UserManager<Utilisateur> userManager;

        public UserController(IServiceUser _serviceUser,UserManager<Utilisateur> _userManager)
        {
            this.serviceUser = _serviceUser;
            this.userManager = _userManager;

        }

        [HttpPost]
        public async Task<IActionResult> addUserAsync([FromBody] RegisterModel register)
        {
            if (!register.valid())
            {
                return BadRequest("Invalid input");
            }

           return Ok( await this.serviceUser.addUserAsync((dynamic)register));
        }

        [HttpPost]
        public async Task<IActionResult> addConsultant([FromBody] ConsultantModel consultant)
        {
            if (!consultant.valid() || string.IsNullOrWhiteSpace(consultant.password))
            {
                return BadRequest("Invalid input");
            }
            return Ok(await serviceUser.addConsultant((dynamic)consultant)??"Invalid input: commercial id incorrect or there is a problem with the server");
        }
        [HttpPut]
        public async Task<IActionResult> updateConsultant(string userid,[FromBody] ConsultantProfile consultant)
        {
            if (!consultant.valid())
            {
                return BadRequest("Invalid input");
            }
            return Ok(await serviceUser.updateConsultant((dynamic)consultant,userid) ?? "Invalid input: user id incorrect or there is a problem with the server");
        }


        [HttpPut]
        public async Task<IActionResult> updateProfile([FromBody] ProfileModel profile)
        {
            if (!profile.valid())
            {
                return BadRequest("Error in the object format (null values)");
            }
            var user = await this.serviceUser.updateProfile((dynamic)profile);

            return user != null ? Ok(user) : BadRequest("Error occured while updating the profile");
        }


        [Authorize(Roles = "Administrateur")]
        [HttpGet]
        [Route("/listCommercial")]
        public async Task<IActionResult> listCommercial(string keyword=null)
        {
            return Ok(await this.serviceUser.listCommercial(keyword));
        }

        [Authorize(Roles = "Administrateur, Commercial")]
        [HttpGet]
        [Route("/listConsultant")]
        public async Task<IActionResult> listConsultant(string keyword = null)
        {
            return Ok(await this.serviceUser.listConsultant(keyword));
        }

        [Authorize(Roles = "Administrateur")]
        [HttpGet]
        [Route("/listRH")]
        public async Task<IActionResult> listRH(string keyword = null)
        {
            return Ok(await this.serviceUser.listGestionnaireRH(keyword));
        } 
        

        [HttpGet]
        public async Task<IActionResult> filter(string filter=null)
        {
            var result = await serviceUser.Search(filter);
            return Ok(result);
        }


        [Authorize(Roles = "Administrateur")]
        [HttpDelete]
        public async Task<IActionResult> deleteUser(string userid)
        {
            var result = await serviceUser.deleteUser(userid);
            if (result)
            {
                return Ok("User deleted successfully");
            }
            else
            {
                return Ok("Error occured try disabling the account");
            }
        }
        [Authorize(Roles = "Administrateur")]
        [HttpDelete]
        public async Task<IActionResult> lockOutUser(string userid)
        {
            var result = await serviceUser.disableAccount(userid);
            if (result)
            {
                return Ok("User account disabled successfully");
            }
            else
            {
                return Ok("Error occured");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("email is required");

            return Ok(await serviceUser.forgotPassword(email));
        
        }

        [HttpPost]
        public async Task<IActionResult> resetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return  Ok(await serviceUser.resetPassword(model.Email, model.Token, model.Password));


        }

    }
}
