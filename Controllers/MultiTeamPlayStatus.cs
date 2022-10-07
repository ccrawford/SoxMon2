using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MultiTeamPlayStatus : ControllerBase
    {
        // GET api/<MultiTeamPlayStatus>/5
        [HttpGet()]
        //public async Task<IEnumerable<MultiGameDTO>> Get([FromQuery] String ids)
        public async Task<String> Get([FromQuery] String ids)
        {
            var mlbClient = new MLBClient();

            List<int> teamIds = ids.Split(',').Select(int.Parse).ToList();

            // var retval = new List<MultiGameDTO>();
            bool allDone = true;
            bool noneStarted = true;

            var queryDate = DateTime.Now;
            var games = await mlbClient.GetScheduleAsync(queryDate);
            // check for null games. 
            foreach (var game in games)
            {
//                var code = await mlbClient.GetScheduleAsync(teamId);
                if (game.AbstractGameCode == "L" || game.AbstractGameCode == "P") allDone = false;
                if (game.AbstractGameCode == "L" || game.AbstractGameCode == "F") noneStarted = false;
            //    retval.Add(new MultiGameDTO {abstractGameCode = game.AbstractGameCode, teamId = game.teamId });
            }

//            retval.Add(new MultiGameDTO { abstractGameCode = allDone ? "F" : noneStarted ? "P" : "L", teamId = 0 });
            String retval = allDone ? "F" : noneStarted ? "P" : "L";

            return retval;

            //String retval = string.Empty;
            //foreach (int id in ids)
            //{
            //    retval += $"id: {id}";
            //}
            //return retval;
        }

    }

    public class MultiGameDTO
    {
        public String? abstractGameCode { get; set; }
        public int teamId { get; set; }
   //     public int nextUpdate { get; set; }
   //     public bool doneForDay { get; set; }
    }
}
