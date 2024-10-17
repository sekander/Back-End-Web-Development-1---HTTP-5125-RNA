using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace MyApiProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class J3Controller : ControllerBase
    {
 	/*
         * <summary>
         * This endpoint decodes secret instructions from a string of comma-separated values.
         * Each value is analyzed to determine the direction (left or right) based on the first two digits 
         * and then appends the remaining digits as the number of steps in that direction.
         * It processes the data until it encounters "99999," which signals the end of the instructions.
         * </summary>
         *
         * <returns>
         * Returns a string containing directions (left or right) and the number of steps, with each instruction on a new line.
         * </returns>
         *
         * <param name="data">
         * The query string contains a comma-separated series of numeric strings. 
         * Each string starts with two digits that are summed to determine the direction (even sum for "right," odd sum for "left"), 
         * and the remaining digits represent the number of steps in that direction. The sequence ends when "99999" is encountered.
         * </param>
         *
         * <example>
         * Example: GET api/J3/secretInstructions?data=12345,67890,99999 would return:
         * "left 345
         * right 890"
         * </example>
         */
        string test;
        
        private List<string> directionList = [];
        // GET api/J3/secretinstructions
        //2021 CCC J3
        [HttpGet("secretInstructions")]
        public IActionResult GetSecretInstructions([FromQuery] string data)
        {
            string[] queryDataStrings = data.Split(',');
            
            foreach(var query in queryDataStrings)
            {
                if (query != "99999")
                {
                    //int firstCharacter = query[0];
                    int firstCharacter = int.Parse( query[0].ToString());
                    int secondCharacter = int.Parse( query[1].ToString());

                    string direction = (firstCharacter + secondCharacter) % 2 == 0 ? "right" : "left";
                    
                    string steps = query.Substring(2);

                    test = direction + " " + steps;
                    directionList.Add(test);
                }
                else
                    break;
            }

            StringBuilder results = new();

            foreach(var dir in directionList)
                results.AppendLine(dir);

            string finalResults = results.ToString().Trim();

            return Ok(finalResults);
        }
    }
}
