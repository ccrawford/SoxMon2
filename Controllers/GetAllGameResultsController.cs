using Microsoft.AspNetCore.Mvc;
using BaseballSharp;
using BaseballSharp.Models;
using System.Linq;
using SoxMon2.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllGameResults : ControllerBase
    {
        // GET api/<GetAllGameResults>/2022-08-01
        [HttpGet("{date}")]
        public async Task<IEnumerable<BaseballSharp.Models.GameSummary>> Get(string date)
        {

            var mlbClient = new MLBClient();
           
            var retval = await mlbClient.GetAllGameFullSummary(date);
            
            return retval;
        }

    }
}

