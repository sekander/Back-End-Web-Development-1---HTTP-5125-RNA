/* <summary>
 * This controller returns a GET request where the endpoint accepts a double parameter 
 * passed through the query.
 * The variable passed through represents the length of a hexagon and the function
 * calculates the area of a hexagon
 *
 * */

using Microsoft.AspNetCore.Mvc;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Q6Controller : ControllerBase
    {
        // GET api/q6/hexagon
        [HttpGet("hexagon")]
        // This funtion takes a double parameter from the query string to calculate 
        // the area of a hexagon.
        public IActionResult GetGreetingMessage([FromQuery] double side)
        {
            //A varaible that holds the area of the hexagon
            double area;

            //Formula to calculate the area of a hexagon with the parameter passed in as the 
            //the lenght of the sides 
            area = ((3 * Math.Sqrt(3))/2) * Math.Pow(side, 2);

            //Returns HTTP 200 ok message with the area calulated and returned 
            return Ok(area);
        }
    }
}
