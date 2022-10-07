using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using SoxMon2.DTOs;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetBoxScoreController : ControllerBase
    {

        // GET api/<GetBoxScore>/5
        [HttpGet("{gamePk}")]
        public async Task<BoxScoreDTO> Get(int gamePk)
        {
            var mlbClient = new MLBClient();

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
            var gameTimeSeconds = new DateTimeOffset(sched.GameTime ?? DateTime.MinValue).ToUnixTimeSeconds();
            var retVal = new BoxScoreDTO()
            {
                CurInning = res.CurrentInning ?? 0,

                HomeAbbr = MLBHelpers.NameToShortName(sched.HomeTeam ?? "XXX"),
                HometeamRunsGame = res.HometeamRunsGame ?? 0,
                HometeamHitsGame = res.HometeamHits ?? 0,
                HometeamErrorsGame = res.HometeamErrors ?? 0,
                AwayAbbr = MLBHelpers.NameToShortName(sched.AwayTeam ?? "XXX"),
                AwayteamRunsGame = res.AwayteamRunsGame ?? 0,
                AwayteamHitsGame = res.AwayteamHits ?? 0,
                AwayteamErrorsGame = res.AwayteamErrors ?? 0,
                InningHalf = res.InningHalf,
                InningState = res.InningState, //Top, Bottom, End
                Outs = res.Outs ?? 0,
                StatusBlurb = "",
                Pitcher = res.DefensePitcherName ?? "",
                Batter = res.OffensiveTeamBatterName ?? "",
                GameStatus = sched.CodedGameState ?? "X",
                GameTime = gameTimeSeconds,
                DayNight = (sched.DayNight ?? "?")=="night" ? "nite" : "day",
                DoW = sched.OfficialDate?.ToString("ddd") ?? "???",
                ManOnFirst = res.ManOnFirst,
                ManOnSecond = res.ManOnSecond,
                ManOnThird = res.ManOnThird,

                // GameDayOfWeek = sched.GameTime?.ToString("ddd"),
            };
            if(sched.CodedGameState == "I")
            {
                //   retVal.StatusBlurb = $"{res.InningHalf?.Substring(0,1)}{res.CurrentInning} {res.Outs}o";
                // On the client, [ is an up arrow and ] is a down arrow in the custom font.
                string arrow = res.InningHalf == "Top" ? "[" : "]";

                retVal.HomeWinProb = await mlbClient.GetHomeTeamWinProbability(gamePk);
                
                retVal.StatusBlurb = $"{arrow}{res.CurrentInning}";
                retVal.LastComment = await mlbClient.GetLatestComment(gamePk);
            }
            if(sched.CodedGameState == "F" || sched.CodedGameState == "O")
            {
                retVal.StatusBlurb = "Final";
                var recap = await mlbClient.GetContentRecap(gamePk);
                var blurb = recap.seoTitle;
                retVal.LastComment = blurb;
            }
            if (sched.CodedGameState != sched.StatusCode)
            {
                retVal.StatusBlurb = $"{sched.DetailedState} {sched.Reason}";
            }
            else if (sched.CodedGameState == "S")
            {
                retVal.StatusBlurb = $"{retVal.DoW} {retVal.DayNight}";
            }
            else if (sched.CodedGameState == "P")
            {
                var startInt = sched.GameTime - DateTime.UtcNow;
                if (startInt.Value.TotalDays > 1)
                    retVal.StatusBlurb = String.Format("in {0}d {1}h {2}m", (int)startInt.Value.Hours, (int)startInt.Value.Minutes);
                else if (startInt.Value.TotalMinutes > 90)
                    retVal.StatusBlurb = String.Format("in {0}h {1}m", (int)startInt.Value.Hours, (int)startInt.Value.Minutes);
                else
                    retVal.StatusBlurb = String.Format("in {0} min", (int)startInt.Value.TotalMinutes);
                
            }

            var maxInning = lineScore.Skip(Math.Max(0, lineScore.Count() - 10)); //Only room for 10 innings.
            foreach (var inning in maxInning)
            {
            
                if (sched.CodedGameState == "P")
                {
                    retVal.AwayLineScore += sched.AwayProbablePitcher;
                    retVal.HomeLineScore += sched.HomeProbablePitcher;
                }
                else
                {
                    if (inning.HometeamRuns == null)
                    {
                        if (sched.CodedGameState != "I" || (res.CurrentInning == inning.InningNumber && res.InningHalf == "Bottom")) retVal.HomeLineScore += "-";
                    }
                    else
                    {
                        retVal.HomeLineScore += (inning.HometeamRuns ?? 0).ToString("X"); //null if current inning and no runs
                    }
                    if (inning.AwayteamRuns == null)
                    {
                        if (res.CurrentInning == inning.InningNumber && res.InningHalf == "Top") retVal.AwayLineScore += "-";
                    }
                    else
                    {
                        retVal.AwayLineScore += (inning.AwayteamRuns ?? 0).ToString("X"); // Format as hex if 10 runs in inning
                    }
                }
            }

            return retVal;
        }

    }
}
