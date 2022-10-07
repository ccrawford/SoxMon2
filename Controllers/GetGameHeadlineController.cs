using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BaseballSharp;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetGameHeadlineController : ControllerBase
    {
        // GET api/<GetGameHeadline>/552233
        [HttpGet("{gamePk}")]
        public async Task<String> Get(int gamePk)
        {
            var mlbClient = new MLBClient();
            String headline = String.Empty;

            var recap = await mlbClient.GetContentRecap(gamePk);

            if (recap != null)
            { 
                headline = recap.seoTitle;
            }

            return headline;
        }
    }
}
