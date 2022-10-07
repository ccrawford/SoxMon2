using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

namespace SoxMon2.Controllers
{
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

            retval.liveGameCount = sched.totalGamesInProgress;

            foreach (var game in sched.dates[0].games)
            {
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
        public bool favTeamLive { get; set; }
        public bool doneForDay { get; set; }
        public bool notStarted { get; set; }
        public char dayState { get; set; }
    }
}
