using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllPostSeasonGameResults : ControllerBase
    {
        // GET api/<GetAllPostSeasonGameResults>/2022-08-01
        [HttpGet("{date}")]
        public async Task<IEnumerable<BaseballSharp.Models.PostSeasonGameSummary>> Get(string date)
        {

            var mlbClient = new MLBClient();

            var retval = await mlbClient.GetPostSeasonFullSummary(date);

            return retval;
        }
    }
}
