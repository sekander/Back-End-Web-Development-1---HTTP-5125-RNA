using Microsoft.AspNetCore.Mvc;

namespace Assignment0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("text/html")]
    public class HelloWorldController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            //return Ok("Hello, World!");
	    var htmlContent = "<html><body><h1>Hello, W0rld!</h1></body></html>";
            return Content(htmlContent, "text/html");
        }
    }
}

