using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using BaseballSharp.Models;
using SoxMon2.DTOs;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WildCardStandingsController : ControllerBase
    {
        // GET api/<GetWildCardStandings>
        [HttpGet()]
        public async Task<IEnumerable<WildcardStanding>> Get()
        {
            var mlbClient = new MLBClient();

            //TODO FIX will need to do this manually. Added stuff to league standings model.

            var standings = await mlbClient.GetWildcardStandings();


            return standings;

        }

    }
}
