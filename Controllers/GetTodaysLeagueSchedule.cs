using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTodaysLeagueSchedule : ControllerBase
    {
        // GET api/<GetGamesForDate>/5
        [HttpGet("{gameDate}")]
        public async Task<IEnumerable<TodaysLeagueScheduleDto>> Get(string gameDate)
        {
            DateTime queryDate;
            var mlbClient = new MLBClient();

            var retVal = new List<TodaysLeagueScheduleDto>();

            try
            {
                queryDate = DateTime.Parse(gameDate);
            }
            catch (FormatException)
            {
                return retVal;
            }


            // Double header logic? Ugh.

            // Today.
            var sched = await mlbClient.GetScheduleAsync(queryDate);
            foreach (var game in sched)
            {
                DateTime gt = (game?.GameTime ?? DateTime.MinValue);
                retVal.Add(new TodaysLeagueScheduleDto
                {
                    teamId = game.AwayID,
                    curGamePk = game.gameID,
                    curGameStart = ((DateTimeOffset)gt).ToUnixTimeSeconds(),
                }) ;
                retVal.Add(new TodaysLeagueScheduleDto
                {
                    teamId = game.HomeID,
                    curGamePk = game.gameID,
                    curGameStart = ((DateTimeOffset)gt).ToUnixTimeSeconds(),

                });
            }

            // Yesterday
            queryDate = queryDate.AddDays(-1);
            sched = await mlbClient.GetScheduleAsync(queryDate);
            foreach (var game in sched)
            {
                var team = retVal.Find(x => x.teamId == game.HomeID && x.lastGamePk == null);
                if (team != null) team.lastGamePk=game.gameID;
                else
                {
                    retVal.Add(new TodaysLeagueScheduleDto
                    {
                        teamId = game.HomeID,
                        curGamePk = 0,
                        curGameStart= 0,
                        lastGamePk = game.gameID,
                    }) ;
                }

                team = retVal.Find(x => x.teamId == game.AwayID && x.lastGamePk == null);
                if (team != null) team.lastGamePk = game.gameID;
                else
                {
                    retVal.Add(new TodaysLeagueScheduleDto
                    {
                        teamId = game.AwayID,
                        curGamePk = 0,
                        curGameStart = 0,
                        lastGamePk = game.gameID,
                    });
                }
            }

            return retVal;
        }
    }

    public class TodaysLeagueScheduleDto
    {
        public int? teamId { get; set; }
        public int? lastGamePk { get; set; }
        public int? curGamePk { get; set; }
        public long? curGameStart { get; set; }
        public int nextGamePk { get; set; }

    }
}


