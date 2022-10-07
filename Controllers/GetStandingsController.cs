using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using SoxMon2.DTOs;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetStandingsController : ControllerBase
    {
        // GET api/<GetNextGameController>/5
        [HttpGet("{teamId}")]
        public async Task<IEnumerable<StandingsDto>> Get(int teamId)
        {
            var mlbClient = new MLBClient();

            var retVal = new List<StandingsDto>();

            var divisionId = BaseballSharp.MLBHelpers.DivisionIdFromTeamId(teamId);

            var standings = await mlbClient.GetStandings(divisionId);
            var sortedStandings = standings.OrderBy(s => s.divisionRank).ToList();

            foreach (var team in sortedStandings)
            {
                retVal.Add(new StandingsDto()
                {
                    divisionRank = team.divisionRank,
                    teamId = team.teamId,
                    displayText = $"{team.divisionRank}. {BaseballSharp.MLBHelpers.NameToShortName(team.name ?? "XXX")} {team.wins}/{team.losses} {team.gamesBack}",
                    gamesBack = (team.gamesBack) ?? "-".PadLeft(5),
                    teamName = BaseballSharp.MLBHelpers.NameToShortName(team.name ?? "XXX"),
                    wins = team.wins,
                    losses = team.losses,

                });
            }
            return retVal;
        }

        [HttpGet()]
        public async Task<IEnumerable<BaseballSharp.Models.DivisionStanding>> Get()
        {
            var mlbClient = new MLBClient();

            //TODO FIX will need to do this manually. Added stuff to league standings model.

            var standings = await mlbClient.GetLeagueStandings();


            return standings;
        }
    }
}
