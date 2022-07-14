using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using BaseballSharp.Models;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeForNextUpdateController : ControllerBase
    {
        [HttpGet()]
        public async Task<double> Get(int teamId)
        {
            var mlbClient = new MLBClient();
            DateTime today = DateTime.UtcNow.AddHours(-5);
            long retVal = 0;
            bool allComplete = true;
            bool someToStart = false;
            DateTime nextStart = today.AddDays(2);

            var games = await mlbClient.GetScheduleAsync(today);
            // Loop and check if any games in progress.
            // Check if any are still to start. 
            // Set time for next future game start if any are scheduled or pre-game.
            //  
            foreach (var game in games)
            {
                if (game.CodedGameState == "I") return 0; //Shortcut if there is a game going on.
                if(game.CodedGameState != "F")
                {
                    allComplete = false;
                }
                if(game.CodedGameState == "S" || game.CodedGameState == "P")
                {
                    someToStart = true;
                    if (game.GameTime < nextStart) nextStart = game.GameTime ?? today;
                }

            }

            if (someToStart) return (long)(nextStart.Subtract(DateTime.UtcNow).TotalSeconds);
            if (allComplete) return (long)((DateTime.UtcNow.Date.AddDays(1) - DateTime.UtcNow).TotalSeconds);
            return 0;
        }

    }
}
