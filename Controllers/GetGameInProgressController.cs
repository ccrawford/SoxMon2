using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetGameInProgressController : ControllerBase
    {
        // GET api/<GetGameInProgress>/5
        [HttpGet("{teamId}")]
        public async Task<int> Get(int teamId)
        {
            var mlbClient = new MLBClient();

            var gameId = await mlbClient.GetGameInProgressId(teamId);

            return gameId;
        }
    }
}
