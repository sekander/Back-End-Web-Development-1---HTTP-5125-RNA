/* <summary>
 * This controller handles a POST request that receives an integer 
 * parameter from the request body and returns it with a message.
 * */

using Microsoft.AspNetCore.Mvc;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Q5Controller : ControllerBase
    {
        // POST api/q5/secret
        [HttpPost("secret")]
        //Parameter that is integer binded to the body a retunred with a message
        public IActionResult Secret([FromBody] int secret)
        {
            // Returns an HTTP 200 OK status code with a message that includes 
            // the parameter value received from the request body.
            return Ok($"Shh.. the secret is {secret}");
        }
    }
}
