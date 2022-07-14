using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using SoxMon2.DTOs;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetUpcomingGameInfoForTeamController : ControllerBase
    {

        // GET api/<GetUpcomingGameInfoForTeam>/5
        [HttpGet("{teamId}")]
        public async Task<UpcomingGameDTO> Get(int teamId)
        {
            var mlbClient = new MLBClient();
            var gamePk = await mlbClient.GetNextUnplayedGameId(teamId);
            var sched = await mlbClient.GetSingleScheduleAsync(gamePk);
            var lineScore = await mlbClient.GetLineScoreAsync(gamePk);
            BaseballSharp.Models.Linescore res;
            if (lineScore.Count() == 0)
            {
                res = new BaseballSharp.Models.Linescore();
            }
            else
            {
                res = lineScore.ToArray()[0];
            }

            var retVal = new UpcomingGameDTO()
            {
                GamePk = gamePk,
                HomeAbbr = MLBHelpers.NameToShortName(sched.HomeTeam ?? "XXX"),
                HomeTeamName = sched.HomeTeam,
                AwayAbbr = MLBHelpers.NameToShortName(sched.AwayTeam ?? "XXX"),
                AwayTeamName = sched.AwayTeam,

                GameTimeUnix = ((DateTimeOffset)sched.GameTime).ToUnixTimeSeconds(),
                HomePitcher = sched.HomeProbablePitcher,
                AwayPitcher = sched.AwayProbablePitcher,

                GameStatus = sched.CodedGameState ?? "X",
                GameTime = sched.GameTime ?? DateTime.MinValue,

                DayNight = (sched.DayNight ?? "?") == "night" ? "nite" : "day",

            };
            if (sched.CodedGameState == "S")
            {
                retVal.StatusBlurb = $"{sched.GameTime.Value.DayOfWeek} {retVal.DayNight}";
            }
            if (sched.CodedGameState == "P")
            {
                var startInt = sched.GameTime - DateTime.UtcNow;
                if (startInt.Value.TotalDays > 1)
                    retVal.StatusBlurb = String.Format("in {0}d {1}h {2}m", (int)startInt.Value.Hours, (int)startInt.Value.Minutes);
                else if (startInt.Value.TotalMinutes > 90)
                    retVal.StatusBlurb = String.Format("in {0}h {1}m", (int)startInt.Value.Hours, (int)startInt.Value.Minutes);
                else
                    retVal.StatusBlurb = String.Format("in {0} min", (int)startInt.Value.TotalMinutes);
            }


            return retVal;
        }

    }
}

