using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using BaseballSharp.Models;
using System.Linq;
using SoxMon2.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameResult : ControllerBase
    {
        // GET api/<GameResult>/5
        [HttpGet("{gamePk}")]
        public async Task<GameResultDTO> Get(int gamePk)
        {
            
            var mlbClient = new MLBClient();

            var sched = await mlbClient.GetSingleScheduleAsync(gamePk);
            var lineScore = await mlbClient.GetLineScoreAsync(gamePk);

            var res = lineScore.ToArray()[0];

            var retVal = new GameResultDTO()
            {
                HomeAbbr = MLBHelpers.NameToShortName(sched.HomeTeam ?? ""),
                HomeRuns = res.HometeamRunsGame ?? 0,
                HomeHits = res.HometeamHits ?? 0,
                HomeErrors = res.HometeamErrors ?? 0,
                AwayAbbr = MLBHelpers.NameToShortName(sched.AwayTeam ?? ""),
                AwayRuns = res.AwayteamRunsGame ?? 0,
                AwayHits = res.AwayteamHits ?? 0,
                AwayErrors = res.AwayteamErrors ?? 0,
                GameDayOfWeek = sched.GameTime?.ToString("ddd"),
                GameState = sched?.CodedGameState ?? "X",
            };

            retVal.Display = $"{retVal.GameDayOfWeek}:{retVal.HomeAbbr}-{retVal.HomeRuns} {retVal.AwayAbbr}-{retVal.AwayRuns}";
            
            return retVal ;
        }
        
    }
}
