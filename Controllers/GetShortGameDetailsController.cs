using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using SoxMon2.DTOs;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetShortGameDetailsController : ControllerBase
    {
        // GET: api/<GetShortGameResult>
        [HttpGet("{gamePk}")]
        public async Task<GameResultDTO> Get(int gamePk)
        {

            var mlbClient = new MLBClient();

            var sched = await mlbClient.GetSingleScheduleAsync(gamePk);

            string? day = "???";
            if (sched.GameTime != null) day = sched?.GameTime?.ToString("ddd") ?? "???";

            GameResultDTO retVal = new GameResultDTO()
            {
                GameDayOfWeek = day,
                GameState = sched?.CodedGameState ?? "X",
                GameTime = sched.GameTime ?? DateTime.MinValue,
                HomeAbbr = MLBHelpers.NameToShortName(sched.HomeTeam),
                AwayAbbr = MLBHelpers.NameToShortName(sched.AwayTeam),
                HomeRuns = sched.HomeScore ?? 0,
                AwayRuns = sched.AwayScore ?? 0,
                DayNight = sched.DayNight ?? "",
               
            };

            retVal.Display = $"{retVal.AwayAbbr} @ {retVal.HomeAbbr} {day} {retVal.DayNight} ";

            return retVal;
        }
    }
}
