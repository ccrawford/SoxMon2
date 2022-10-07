using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using SoxMon2.DTOs;
using BaseballSharp.Models;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetGamesForDateController : ControllerBase
    {
        // GET api/<GetGamesForDate>/5
        [HttpGet("{gameDate}")]
        public async Task<IEnumerable<GamesForDateDTO>> Get(string gameDate)
        {
            DateTime queryDate;
            var mlbClient = new MLBClient();

            var retVal = new List<GamesForDateDTO>();


            try
            {
                queryDate = DateTime.Parse(gameDate);
            }
            catch (FormatException)
            {
                return retVal;
            }

            var games = await mlbClient.GetScheduleAsync(queryDate);
            foreach (var game in games)
            {
                BaseballSharp.Models.Linescore gameInfo = new Linescore();
                if (game != null && game.CodedGameState == "I")
                {
                    var gameInfos = await mlbClient.GetLineScoreAsync(game.gameID ?? 0);
                    if (gameInfos != null) { gameInfo = gameInfos.FirstOrDefault(); }
                }
                retVal.Add(new GamesForDateDTO
                {
                    HomeAbbr = MLBHelpers.NameToShortName(game.HomeTeam ?? "XXX"),
                    HomeTeamRuns = game.HomeScore,
                    AwayAbbr = MLBHelpers.NameToShortName(game.AwayTeam ?? "XXX"),
                    AwayTeamRuns = game.AwayScore,
                    Inning = "",
                    
                    GameStatus = game.CodedGameState,
   //                  GameTime = game.GameTime,
                    GameTimeUnix = ((DateTimeOffset)game.GameTime).ToUnixTimeSeconds(),

                });

                if (game.CodedGameState == "F" || game.CodedGameState == "O") retVal.Last().Inning = "F";
                if (game.CodedGameState == "I") retVal.Last().Inning = (gameInfo ?? new Linescore()).CurrentInning.ToString();
                if (game.CodedGameState == "S" || game.CodedGameState == "P") retVal.Last().Inning = game.DayNight.Substring(0, 1);
                if (game.CodedGameState == "D") retVal.Last().Inning = "X";

            }

            return retVal;
        }
    }
}
