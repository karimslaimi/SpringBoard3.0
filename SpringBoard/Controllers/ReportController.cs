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

        [SwaggerOperation(Summary = "get report by date", Description = "this methode to search reports(compte rendu) by date please be carefull the date format must be dd-MM-yyyy")]
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


        [SwaggerOperation(Summary = "get report by user", Description = "this methode to search reports(compte rendu) by user id ")]

        [HttpGet]
        public async Task<IActionResult> getUserReport(string userId)
        {
            return Ok(await serviceCompteRendu.getUserCR(userId));
        }


        [SwaggerOperation(Summary = "get report by user and date", Description = "this methode to search reports(compte rendu) by user id date please be carefull the date format must be dd-MM-yyyy")]

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


        [SwaggerOperation(Summary = "validate report", Description = "this methode to validate reports(compte rendu) ")]

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
        [SwaggerOperation(Summary = "unlock report", Description = "when the report(compte rendu) is validated then it will be locked and can't be updated this method to unlock it")]


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
        [SwaggerOperation(Summary = "delete report", Description = "Attention when deleting a report it can't be undone")]


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
