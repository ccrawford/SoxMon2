using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWS.WeatherDataService;
using ZipToLatLon;

namespace SoxMon2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetCurrentWeatherController : ControllerBase
    {
        [HttpGet(Name = "GetCurrentWeather")]
        public async Task<CurConditionsDto> Get([FromQuery] decimal lat, decimal lon, string zip)
        {
            if (zip != null)
            {
                ZipLookup zips = ZipLookup.Instance;
                ZipLatLon latlon = zips.GetLatLon(zip);
                lat = latlon.lat;
                lon = latlon.lon;
            }
            var nws = new NWS.WeatherDataService.WeatherDataService();
            NWS.Model.CurrentConditionsResponse curCond = await nws.GetCurrentConditionsAsync(lat, lon);
            var retval = new CurConditionsDto()
            {
                curTempF = Math.Round(curCond.TemperatureFahrenheit ?? -99, 0),
                textDescription = curCond.TextDescription,
                observationDttm = curCond.ObservationDate,
                observationStation = curCond.Station.Name,
            };

            return retval;
        }
    }
}


public class CurConditionsDto
{
    public decimal curTempF { get; set; }
    public string textDescription { get; set; }
    public DateTime observationDttm { get; set; }
    public string observationStation { get; set; }
}
