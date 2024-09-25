/* <summary>
 * A simple controller that returns a GET Request and returns a simple welcome static message
 * */

//Import namespace for ASP.NET
using Microsoft.AspNetCore.Mvc;

namespace MyApiProject.Controllers
{
    //Marks this class as an API controller for handling HTTP requests
    [ApiController]

    //The base url for this controller
    [Route("api/[controller]")]
    public class Q1Controller : ControllerBase
    {
        //Define GET Request endpoint for "api/q1/welcome"
        // GET api/q1/welcome
        // [HttpGet("welcome")] attribute specifies that this action responds to GET Resquest
        [HttpGet("welcome")]
        //An interface in ASP.NET that allows action methods to return 
        //different types of HTTP responces
        public IActionResult GetWelcomeMessage()
        {
            // Return an HTTP 200 OK response with a simple message
            return Ok("Welcome to 5125!");
        }
    }
}
