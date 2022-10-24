using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

namespace SoxMon2.Controllers
{
    // Figures out what's going on currently so we can set the right state in the 
    //  local device. E.g. favorite team live. Or off season. Or waiting for game to start.
    // Takes a TeamId so we can check if that team is live.
    // Called on a schedule based on expected time of next change.

    [Route("api/[controller]")]
    [ApiController]
    public class GetFullDayStateController : ControllerBase
    {
        [HttpGet()]
        public async Task<FullDayStateDto> Get([FromQuery] int? teamId, [FromQuery] String? date = null)
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

            FullDayStateDto retval = new FullDayStateDto(){
                favTeamLive = false,
                liveGameCount = 0,
                doneForDay = false,
                notStarted = false,
                dayState = '.',
                seasonState = ".",
            };
            bool allDone = true;
            bool noneStarted = true;

            var sched = await mlbClient.GetFullScheduleAsync(schedDate);

            // check for null games. 
            if (sched == null || sched.totalGames == 0)
            {
                retval.dayState = '0'; // No games today
                return retval;
            }

            var seasonDates = await mlbClient.GetSeasonDates(103);  //103 => American league. Arbitrary.
            // Season states: 
            // postseason offseason preseason regularseason(?) allstar(?) spring(?)
            retval.seasonState = seasonDates.seasonState;
            retval.liveGameCount = sched.totalGamesInProgress;
            retval.onlyLiveGamePk = 0;
            
            foreach (var game in sched.dates[0].games)
            {
                
                if (sched.totalGamesInProgress == 1 && game.status.abstractGameCode == "L")
                    retval.onlyLiveGamePk = game.gamePk;
                
                var home = game.teams.home.team.id;
                var away = game.teams.away.team.id;
                var agc = game.status.abstractGameCode;

                if ((home == teamId || away == teamId) && agc == "L")
                {
                    retval.dayState = '*';
                    retval.favTeamLive = true;
                }
                if (agc == "L" || agc == "P") allDone = false;
                if (agc == "L" || agc == "F") noneStarted = false;
            }
            if (allDone) retval.doneForDay = true;
            if (noneStarted) retval.notStarted = true;
            if (retval.dayState == '.')
            {
                retval.dayState = allDone ? 'F' : noneStarted ? 'P' : 'L';
            }

            return retval;
        }

    }

    public class FullDayStateDto
    {
        public int liveGameCount { get; set; }
        public int onlyLiveGamePk { get; set; }
        public bool favTeamLive { get; set; }
        public bool doneForDay { get; set; }
        public bool notStarted { get; set; }
        public char dayState { get; set; }
        public string seasonState { get; set; }
    }
}
