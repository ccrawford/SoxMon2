using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetNextGameIdController : ControllerBase
    {

        // GET api/<GetNextGameController>/5
        [HttpGet("{teamId}")]
        public async Task<int> Get(int teamId)
        {
            var mlbClient = new MLBClient();

            var gameId = await mlbClient.GetNextGameId(teamId);

            return gameId;
        }

    }
}
