/*
 * <summary>
 * This controller handles a GET request and computes the cube value
 * of the number that is passed through the URL as a route parameter.
 *
 * */

using Microsoft.AspNetCore.Mvc; 

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Q3Controller : ControllerBase
    {
        // GET api/q3/cube
        // Http Get request that takes the value through the route of the url 
        [HttpGet("cube/{base}")]
        // The parameter is passed through the route of the url it takes 
        // an integer value 
        public IActionResult GetCube([FromRoute] int @base)
        {
            // Returns cube value of the number that was passed through the route 
            return Ok(Math.Pow(@base, 3)); 
        }
    }
}
