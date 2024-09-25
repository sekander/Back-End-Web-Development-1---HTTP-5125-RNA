/* 
 * <summary>
 * This controller handles a POST request that retrieves two variables 
 * from a form submission. These variables represent the quantities 
 * of items to be calculated, and the controller computes the subtotal, 
 * tax, and final total.
 * */


using Microsoft.AspNetCore.Mvc;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Q8Controller : ControllerBase
    {
        //Constant varaible declared as double that represents the prices of the items
        const double small = 25.50;
        const double large = 40.50;

        // POST api/q8/squashfellows
        [HttpPost("squashfellows")]
        //The function that takes two parameters both as integers which represents the amount 
        //of items provided to be calculated
        public IActionResult SquashFellows([FromForm] int Small, [FromForm] int Large)
        {
            //Calculates the sub total rounds the decimal to two decimal points
            double subTotal = Math.Round((small * Small) + (large * Large), 2);
            //Calculates the tax of the subtotal and stores the value
            double tax = Math.Round(subTotal * 0.13, 2);
            //Calculates the final total adds the subtotal and tax 
            double total = Math.Round(subTotal + tax, 2);
            
            //Constructs a return message detailing the quantities entered, 
            //the subtotal, tax, and final total, formatted to two decimal places.

            String returnMessage = $"{Small}" + " @ Small " + small.ToString("F2") + " = $" + (small * Small).ToString("F2") + "; " +
                                   $"{Large}" + " @ Large " + large.ToString("F2") + " = $" + (large * Large).ToString("F2") + ";" + 
                                   " Subtotal = $" + subTotal.ToString("F2") + ";" + " Tax = $" + tax.ToString("F2") + " HST;" + 
                                   " Total = $" + total.ToString("F2");

            //Returns the formatted message as an HTTP 200 OK response.
            return Ok(returnMessage);
        }
    }
}
