using Microsoft.AspNetCore.Mvc;

namespace MyApiProject.Controllers
{

//A general <summary> of what the endpoint does
//A description of what the endpoint <returns>
//A description of each input <param>eter
//At least one <example>

    [ApiController]
    [Route("api/[controller]")]
    public class J1Controller : ControllerBase
    {
	/*
         * <summary>
         * This endpoint calculates the final score for a delivery droid based on the number of deliveries and collisions.
         * It adds 50 points per delivery and subtracts 10 points per collision. Additionally, a bonus of 500 points is 
         * awarded if the number of deliveries exceeds the number of collisions.
         * </summary>
         *
         * <returns>
         * Returns the final calculated score as an integer.
         * </returns>
         *
         * <param name="Collisions">
         * The number of collisions encountered by the delivery droid.
         * </param>
         * <param name="Deliveries">
         * The number of successful deliveries made by the delivery droid.
         * </param>
         *
         * <example>
         * Example: POST api/J1/Delivedroid with Collisions=2 and Deliveries=10 will return 980.
         * </example>
         */
        // Post api/J1/Delivedroid
        [HttpPost("Delivedroid")]
        public IActionResult PostDeliveDroid([FromForm] int Collisions, [FromForm] int Deliveries)
        {
            int finalScore;
            finalScore = ((Deliveries * 50) - (Collisions * 10)) + 
                         (Deliveries > Collisions ? 500 : 0) ;
            // Return a simple message
            return Ok(finalScore);
        }

        /*
         * <summary>
         * This endpoint calculates the atmospheric pressure based on the given water temperature.
         * The formula used is 5 * temperature - 400. The result is compared to 100 to determine 
         * whether the pressure is below, equal to, or above 100.
         * </summary>
         *
         * <returns>
         * Returns:
         * - 1 if the pressure is below 100,
         * - 0 if the pressure is equal to 100,
         * - -1 if the pressure is above 100.
         * </returns>
         *
         * <param name="temperature">
         * The water temperature (in degrees Celsius) used to calculate atmospheric pressure.
         * </param>
         *
         * <example>
         * Example: POST api/J1/BoilingWater with temperature=85 will return -1.
         * Example: POST api/J1/BoilingWater with temperature=100 will return 0.
         * </example>
         */
        //2021 CCC J1
        [HttpPost("BoilingWater")]
        public IActionResult PostBoilingWater([FromForm] int temperature)
        {
            int atmosphericPressure;
            
            atmosphericPressure = 5 * temperature - 400;
            
            if(atmosphericPressure < 100)
                return Ok(1);
            else if (atmosphericPressure == 100)
                return Ok(0);
            else
                return Ok(-1);
        }
    }
}
