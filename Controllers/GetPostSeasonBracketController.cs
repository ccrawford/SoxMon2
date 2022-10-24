using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

namespace SoxMon2.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class GetPostSeasonBracket : ControllerBase
        {
        // GET api/<GetPostSeasonBracket>/2022-08-01
        [HttpGet("{date}")]
            public async Task<IEnumerable<BaseballSharp.Models.PostSeasonSeriesGame>> Get(string date)
            {

                var mlbClient = new MLBClient();

                var retval = await mlbClient.GetPostSeasonBracket(date);

                return retval;
            }
        }

}
