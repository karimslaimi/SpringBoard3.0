using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpringBoard.Service;
using System;
using System.Threading.Tasks;

namespace SpringBoard.API.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {

        private IServiceCompteRendu serviceCompteRendu;


        public ReportController(IServiceCompteRendu _serviceCompteRendu)
        {
            this.serviceCompteRendu = _serviceCompteRendu;

        }


        [HttpPost]
        [Authorize(Roles ="Consultant")]
        public async Task<IActionResult> addRepport(string date,double valeur, string userid)
        {
            if(string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(userid))
            {
                return BadRequest("null values are not accepted");
            }
            DateTime oDate ;
            try
            {

             oDate= DateTime.ParseExact(date, "dd-MM-yyyy", null);

            }catch(Exception e)
            {
                return BadRequest("date format is incorrect (must be dd-MM-yyyy ex: 18-05-2022");
            }


            return  Ok(await serviceCompteRendu.addRapportToCR(oDate, valeur, userid));
        }

        [HttpGet]
        public async Task<IActionResult> getReportByDate(string date)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return BadRequest("null value is not accepted");
            }


            DateTime oDate;
            try
            {

                oDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

            }
            catch (Exception e)
            {
                return BadRequest("date format is incorrect (must be dd-MM-yyyy ex: 18-05-2022");
            }


            return Ok(await serviceCompteRendu.getCRbyDate(oDate));

        }

        [HttpGet]
        public async Task<IActionResult> getUserReport(string userId)
        {
            return Ok(await serviceCompteRendu.getUserCR(userId));
        }

        [HttpGet]
        public async Task<IActionResult> getReportByUserAndDate(string date, string userId)
        {
            if (string.IsNullOrWhiteSpace(date))
            {
                return BadRequest("null value is not accepted");
            }


            DateTime oDate;
            try
            {

                oDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

            }
            catch (Exception e)
            {
                return BadRequest("date format is incorrect (must be dd-MM-yyyy ex: 18-05-2022");
            }

            return Ok(await serviceCompteRendu.GetCRbyDateAndUser(oDate, userId));

        }


        [HttpPut]
        [Authorize(Roles = "Administrateur, Commercial")]

        public async Task<IActionResult> validate(int id)
        {
            var result = await serviceCompteRendu.validateCR(id);
            if (result == null)
            {
                return Ok("report not found");
            }
            else
            {
                return Ok(result);
            }

        }


        [Authorize(Roles = "Administrateur, Commercial")]
        [HttpPut]
        public async Task<IActionResult> unlock(int id)
        {
            var result = await serviceCompteRendu.unlockCR(id);
            if (result == null)
            {
                return Ok("report not found");
            }
            else
            {
                return Ok(result);
            }

        }
        [Authorize(Roles = "Administrateur, Commercial")]
        [HttpDelete]
        public async Task<IActionResult> delete(int id)
        {
            bool result = await serviceCompteRendu.delete(id);
            if (result == false)
            {
                return Ok("report not found");
            }
            else
            {
                return Ok(result);
            }
        }



    }
}
