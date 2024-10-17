using System.ComponentModel;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class J2Controller : ControllerBase
    {

        // Create a Dictionary with string keys and int values
        private readonly Dictionary<string, int> peppers = new()
        {
            {"Poblano",  1500},
            {"Mirasol",  6000},
            {"Serrano",  15500},
            {"Cayenne",  40000},
            {"Thai",     75000},
            {"Habanero", 125000}
        };

	 /*
         * <summary>
         * This endpoint calculates the total spice level of various chili peppers based on the given list of ingredients.
         * The spice level for each pepper is fetched from the `peppers` dictionary and summed up.
         * </summary>
         *
         * <returns>
         * Returns the total spice level as an integer representing the sum of the 
	 * Scoville heat units (SHU) of the peppers in the input.
         * </returns>
         *
         * <param name="Ingredients">
         * A comma-separated list of ingredients (pepper names) passed as a query parameter.
         * Only peppers listed in the dictionary are considered for calculating the spice level.
         * </param>
         *
         * <example>
         * Example: GET api/J2/ChilliPeppers?Ingredients=Serrano,Cayenne will return 55500.
         * </example>
         */
        // GET api/J2/ChilliPeppers
        [HttpGet("ChilliPeppers")]
        public IActionResult GetChilliPeppers([FromQuery] string Ingredients)
        {
            //Total Spice
            int totalSpiceLevel = 0;

            string[] queryIngredients = Ingredients.Split(',');
            
            foreach(var query in queryIngredients)
            {
                if(peppers.ContainsKey(query))
                    totalSpiceLevel += peppers[query];
            }

            // Return a simple message
            return Ok(totalSpiceLevel);
            
        }

	  /*
         * <summary>
         * This endpoint determines the winner of a silent auction by finding the highest bid from the given input data.
         * It parses the input data, stores the bid amounts, and returns the name of the person with the highest bid.
         * </summary>
         *
         * <returns>
         * Returns the name of the person with the highest bid.
         * </returns>
         *
         * <param name="data">
         * A comma-separated string where the first value represents the number of bids, 
	 * and subsequent values alternate between names and bid amounts.
         * </param>
         *
         * <example>
         * Example: GET api/J2/SilentAuction?data=3,John,150,Sara,200,Mark,175 will return "Sara".
         * </example>
         */
        //2021 CCC J2
        [HttpGet("SilentAuction")]
        public IActionResult GetSilentAuction([FromQuery] string data)
        {
            string[] queryData = data.Split(',');
            
            int amountOfBids = int.Parse( queryData[0]);
            
            // Create a Dictionary with string keys and int values
            Dictionary<string, int> bids = [];

            for(int i = 1; i <= amountOfBids; i++)
            {
                if(i < 2)
                    bids.Add(queryData[i], int.Parse(queryData[i +1]));
                else
                    bids.Add(queryData[(i * 2) -1], int.Parse(queryData[i * 2 ]));
            }

            var sortByBid = bids.OrderByDescending(b => b.Value).ToList();

            return Ok(sortByBid[0].Key);
        }
    }
}
