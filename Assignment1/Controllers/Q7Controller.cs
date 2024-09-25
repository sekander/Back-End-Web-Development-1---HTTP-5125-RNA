/* <summary>
 * This controller handles a GET request that calculates a future date 
 * based on the number of days provided as a query parameter.
 * */

using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Q7Controller : ControllerBase
    {
        // GET api/q7/timemachine
        [HttpGet("timemachine")]
        // GET Request which takes an integer days from the query through the url,
        // representing the number of days to add to the current date.
        public IActionResult GetGreetingMessage([FromQuery] int days)
        {
            //Gets the current date and holds in the current date in the varaible decalred
            DateTime currentDate = DateTime.Now;

            //Adjusted Date holds the new date which returns the added days or 
            //substracted days that is passed through the query 
            DateTime adjustedDate = currentDate.AddDays(days);
            
            //Formates the date in a string followed by year month and day
            String newDate = adjustedDate.ToString("yyyy-MM-dd");

            //Returns the new date as a string
            return Ok(newDate);
        }
    }
}
