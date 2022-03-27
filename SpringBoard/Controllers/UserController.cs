using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpringBoard.API.Model;
using SpringBoard.Domaine;
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
                return Ok(null);
            }

           return Ok( await this.serviceUser.addUserAsync((dynamic)register));
        }

        [HttpPut]
        public async Task<IActionResult> updateProfile([FromBody] ProfileModel profile)
        {
            if (!profile.valid())
            {
                return BadRequest("Error in the object format (null values)");
            }
            var user = await this.serviceUser.updateProfile((dynamic)profile, this.userManager);

            return user != null ? Ok(user) : BadRequest("Error occured while updating the profile");
        }


        [Authorize(Roles ="Administration")]
        [HttpGet]
        [Route("/listCommercial")]
        public async Task<IActionResult> listCommercial()
        {
            return Ok(await this.serviceUser.listCommercial());
        }

        [Authorize(Roles = "Administration, Commercial")]
        [HttpGet]
        [Route("/listConsultant")]
        public async Task<IActionResult> listConsultant()
        {
            return Ok(await this.serviceUser.listConsultant());
        }

        [Authorize(Roles = "Administration")]
        [HttpGet]
        [Route("/listRH")]
        public async Task<IActionResult> listRH()
        {
            return Ok(await this.serviceUser.listGestionnaireRH());
        } 
        

        [HttpGet]
        public async Task<IActionResult> filter(string filter)
        {
            var result = await serviceUser.Search(filter);
            return Ok(result);
        }


        [Authorize(Roles = "Administration")]
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
        [Authorize(Roles = "Administration")]
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
    }
}
