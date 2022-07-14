using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTeamsController : ControllerBase
    {
        // GET api/GetTeams/
        [HttpGet()]
        public void Get()
        {
            MLBHelpers.BuildTeamSelect();
        }
    }
}
