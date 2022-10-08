using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using BaseballSharp.Models;
using System.Linq;
using SoxMon2.DTOs;

// Called when we're displaying the pretty game info.
// Returns a complete list of the current game results/live summary, etc. for the date.

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllGameResults : ControllerBase
    {
        // GET api/<GetAllGameResults>/2022-08-01
        [HttpGet("{date}")]
        public async Task<IEnumerable<BaseballSharp.Models.GameSummary>> Get(string date)
        {

            var mlbClient = new MLBClient();
           
            var retval = await mlbClient.GetAllGameFullSummary(date);
            
            return retval;
        }

    }
}

