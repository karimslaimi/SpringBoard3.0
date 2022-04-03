using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpringBoard.Model;
using SpringBoard.Service;
using Swashbuckle.AspNetCore.Annotations;
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


        [SwaggerOperation(Summary ="Add a new report",Description ="this methode to add a report please be carefull the date format must be dd-MM-yyyy")]
        [HttpPost]
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

            ReportModel model = new ReportModel();
            //map the result into the model so we can avoid the circular reference exception
            model = model.Mapper(await serviceCompteRendu.addRapportToCR(oDate, valeur, userid));

            return  Ok(model);
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


        //[Authorize(Roles = "Administrateur, Commercial")]
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

       // [Authorize(Roles = "Administrateur, Commercial")]
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
