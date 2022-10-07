using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCurrentDayStateController : ControllerBase
    {
        [HttpGet()]
        public async Task<String> Get([FromQuery] int? teamId, [FromQuery] String? date = null)
        {
            DateTime schedDate;
            if (teamId == null) teamId = 0;

            if (date == null)
            {
                schedDate = DateTime.UtcNow.AddHours(-6); // Something US reasonable.
            }
            else
            {
                schedDate = DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            var mlbClient = new MLBClient();

            bool allDone = true;
            bool noneStarted = true;

            var games = await mlbClient.GetScheduleAsync(schedDate);
            // check for null games. 
            if (games.Count() == 0) return "0"; // No games today

            foreach (var game in games)
            {
                if ((game.HomeID == teamId || game.AwayID == teamId) && game.AbstractGameCode == "L") return "*";
                if (game.AbstractGameCode == "L" || game.AbstractGameCode == "P") allDone = false;
                if (game.AbstractGameCode == "L" || game.AbstractGameCode == "F") noneStarted = false;
            }

            String retval = allDone ? "F" : noneStarted ? "P" : "L";

            return retval;
        }

    }

}

