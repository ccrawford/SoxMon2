using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using BaseballSharp.DTO;
using SoxMon2.DTOs;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayoffSeeds : ControllerBase
    {
        // GET api/
        [HttpGet()]
        public async Task<List<PlayoffSeedDto>> Get()
        {

            var mlbClient = new MLBClient();
            var standings = await mlbClient.GetLeagueStandings();
            var WCstandings = await mlbClient.GetWildcardStandings();

            var retVal = new List<PlayoffSeedDto>();

            foreach (var division in standings)
            {
                // Get the top team in each division.
                var topTeam = division.Teams.FirstOrDefault();
                retVal.Add(new PlayoffSeedDto
                {
                    teamId = topTeam.teamId,
                    abbr = topTeam.nameAbbr,
                    leagueId = division.LeagueId,
                    winLossPct = (double)topTeam.wins/(double)topTeam.losses,
                    clinched = topTeam.clinched,
                    isWc = false,
                });
                
            }

            foreach (var wcL in WCstandings)
            {
                var topTeam = wcL.Teams.Take(3); //First three teams get it.
                foreach (var team in topTeam)
                {
                    retVal.Add(new PlayoffSeedDto
                    {
                        teamId = team.teamId,
                        abbr = team.nameAbbr,
                        leagueId = wcL.LeagueId,
                        winLossPct = (double)team.wins / (double)team.losses,
                        clinched = team.clinched,
                        isWc = true,
                    });
                }
            }

            int i = 1;
            foreach (var record in retVal.Where((x) => (x.isWc == false) && (x.leagueId == 103)).OrderByDescending(x => x.winLossPct))
            {
                record.seedOrdinal = i++;
            }
            i = 4;
            foreach (var record in retVal.Where((x) => (x.isWc == true) && (x.leagueId == 103)).OrderByDescending(x => x.winLossPct))
            {
                record.seedOrdinal = i++;
            }
            i = 1;
            foreach (var record in retVal.Where((x) => (x.isWc == false) && (x.leagueId == 104)).OrderByDescending(x => x.winLossPct))
            {
                record.seedOrdinal = i++;
            }
            i = 4;
            foreach (var record in retVal.Where((x) => (x.isWc == true) && (x.leagueId == 104)).OrderByDescending(x => x.winLossPct))
            {
                record.seedOrdinal = i++;
            }
            var sortedRet = retVal.OrderBy((x) => x.leagueId).ThenBy((x) => x.seedOrdinal).ToList();
            return sortedRet;
        }
    }

    public class PlayoffSeedDto
    {
        public int teamId { get; set; }
        public string abbr { get; set; }
        public int leagueId { get; set; }
        public int seedOrdinal { get; set; }
        public double winLossPct { get; set; }
        public bool? clinched { get; set; }
        public bool isWc { get; set; }
    }

}
