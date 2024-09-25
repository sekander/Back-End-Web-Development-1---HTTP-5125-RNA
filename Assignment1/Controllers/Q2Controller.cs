/* <summary>
 * This controller handles a GET request where the user can supply a query parameter 
 * through the URL. The controller processes the query parameter and returns a greeting 
 * message that includes the provided argument
 *
 * */

using Microsoft.AspNetCore.Mvc;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Q2Controller : ControllerBase
    {
        // GET api/q2/greeting
        [HttpGet("greeting")]
        //The function called through the controller it takes a query
        //from the url as a parameter that is a string
        public IActionResult GetGreetingMessage([FromQuery] string name)
        {
            // A variable declared which prints message with the parameter
            // that is passed through the url and printed
            var greetingMessage = $"Hi, {name}";

            //Returns an HTTP 200 response takes the variable declared above
            //returns the varaible with the greeting message
            return Ok(greetingMessage);
        }
    }
}
