using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using BaseballSharp.Models;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetLastGameResult : ControllerBase
    {
        // GET: api/<GameResult>
        [HttpGet]
        public async Task<string> Get(int teamId)
        {

            var mlbClient = new MLBClient();
            var lastGamePk = await mlbClient.GetLastGameId(teamId);

            var sched = await mlbClient.GetSingleScheduleAsync(lastGamePk);

            string day;
            if(sched.GameTime != null) day = sched?.GameTime?.ToString("ddd");

            var retVal = ($"{sched.HomeTeam}-{sched.HomeScore} {sched.AwayTeam}-{sched.AwayScore}");

            return retVal ;

        }
    }
}
