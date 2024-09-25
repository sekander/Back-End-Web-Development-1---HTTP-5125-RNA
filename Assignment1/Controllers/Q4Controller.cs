/* <summary>
 * This controller requests a post method that returns the beggining of a knockknock joke
 *
 * */

using Microsoft.AspNetCore.Mvc;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Q4Controller : ControllerBase
    {
        // POST api/q4/knocknock
        // This is a POST requests that send information to the server
        // No Body and No Headers is required for this function
        // The function returns a static message without any input required 
        [HttpPost("knockknock")]
        public IActionResult KnockKnock()
        {
            //Returns the HTTP 200 OK status code with the message provided 
            return Ok("Who's there?");
        }
    }
}
