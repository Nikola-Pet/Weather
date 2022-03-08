using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using Orion.WeatherApi.Models;
using Orion.WeatherApi.Repository;
using Orion.WeatherApi.Services;
using Orion.WeatherApi.DTO;
using Newtonsoft.Json;

namespace Orion.WeatherApi.Controllers
{
    [Authorize]
    public class WeathersController : ApiController
    {
        private AuthenticateService authenticateService;
        private WeatherService weatherServices;
        private IHistoryRepository hIstoryRepository;

        public WeathersController()
        {
            this.weatherServices = new WeatherService();
            this.hIstoryRepository = new HistoryRepository();
            this.authenticateService = new AuthenticateService();
        }
       
        [Route("api/Weathers/ByCity")]
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByCityName([FromBody] SearchCity searchCity, HttpRequestMessage request)
        {
            string token = authenticateService.GetJwtTokenString(request);

            string username = authenticateService.ValidateUsernameJwtToken(token);

            var weather = await weatherServices.GetWeatherByCityName(searchCity.cityName);

            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = username;
            historyModel.SearchRequest = "ByCity";
            historyModel.Data = searchCity.cityName;

            try
            {
                if (weather.name != null)
                {
                    historyModel.Type = "Ok";
                    historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

                    return Ok(weather);
                }
                else
                {
                    string message = "The city was not found or the city name is incorrect";

                    historyModel.Type = "BadRequest";
                    historyModel.Response = message;

                    return BadRequest(message);
                }
            }
            finally
            {
                hIstoryRepository.AddHistory(historyModel);
            }
        }

        [Route("api/Weathers/ByLatLon")]
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByLonLat([FromBody] SearchLatLon searchLatLon, HttpRequestMessage request)
        {
            string token = authenticateService.GetJwtTokenString(request);

            string username = authenticateService.ValidateUsernameJwtToken(token);

            var weather = await weatherServices.GetWeatherByLonLat(searchLatLon.Latitude, searchLatLon.Longitude);

            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = username;
            historyModel.SearchRequest = "Lat/Lon";
            historyModel.Data = searchLatLon.Latitude.ToString() + ", " + searchLatLon.Longitude.ToString();
            historyModel.Response = JsonConvert.SerializeObject(weather).ToString();
            historyModel.Type = "Ok";
            historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

            return Ok(weather);
        }

        [Route("api/Weathers/ByCityId")]
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByCityId([FromBody] SearchCityId searchCityId, HttpRequestMessage request)
        {
            string token = authenticateService.GetJwtTokenString(request);

            string username = authenticateService.ValidateUsernameJwtToken(token);

            var weather = await weatherServices.GetWeatherByCityId(searchCityId.CityId);

            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = username;
            historyModel.SearchRequest = "CityId";
            historyModel.Data = searchCityId.CityId.ToString();

            try
            {
                if (weather.name != null)
                {
                    historyModel.Type = "Ok";
                    historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

                    return Ok(weather);
                }
                else
                {
                    string message = "The cityId is incorrect";

                    historyModel.Type = "BadRequest";
                    historyModel.Response = message;

                    return BadRequest(message);
                }
            }
            finally
            {
                hIstoryRepository.AddHistory(historyModel);
            }
        }
    }
}
