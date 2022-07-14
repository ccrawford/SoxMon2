using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetNextScheduledGameController : ControllerBase
    {

        // GET api/<GetNextGameController>/5
        [HttpGet("{teamId}")]
        public async Task<int> Get(int teamId)
        {
            var mlbClient = new MLBClient();

            var gameId = await mlbClient.GetNextUnplayedGameId(teamId);

            return gameId;
        }

    }
}
