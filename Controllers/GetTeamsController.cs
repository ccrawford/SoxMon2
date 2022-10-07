using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using SoxMon2.DTOs;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTeamsController : ControllerBase
    {
        // GET api/
        [HttpGet()]
        public List<TeamDto> Get()
        {

            var mlbClient = new MLBClient();
            var teams = MLBHelpers.AllTeams();

            var retVal = new List<TeamDto>();

            foreach (var team in teams)
            {
                retVal.Add(new TeamDto
                {
                    Abbr = team.Abbreviation,
                    Id = team.Id,
                    Name = team.TeamName,
                    DivId = team.DivisionId,
                    LeagueId = team.LeagueId,
                    City = team.Location,
                });

            }
            return retVal;
        }
    }
}
