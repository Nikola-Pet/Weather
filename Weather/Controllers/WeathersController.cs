using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using Orion.WeatherApi.Models;
using Orion.WeatherApi.Repository;
using Orion.WeatherApi.Services;
using Orion.WeatherApi.DTO;
using Newtonsoft.Json;
using Orion.WeatherApi.JWT;
using System;

namespace Orion.WeatherApi.Controllers
{
    [CustomAuthenticationFilter]
    public class WeathersController : ApiController
    {
        private WeatherService weatherServices;
        private IHistoryRepository hIstoryRepository;
        private IpAddressService ipAddressService;

        public WeathersController()
        {
            this.weatherServices = new WeatherService();
            this.hIstoryRepository = new HistoryRepository();
            this.ipAddressService = new IpAddressService();
        }
       
        [Route("api/Weathers/ByCity")]
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByCityName([FromBody] SearchCity searchCity)
        {
            var request = Request.Headers.Authorization;

            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = AuthenticateService.GetUsernameFromJWT(request.ToString());
            historyModel.IPAddress = ipAddressService.GetIp();
            historyModel.SearchRequest = "ByCity";
            historyModel.Data = searchCity.cityName;

            try
            {
                var weather = await weatherServices.GetWeatherByCityName(searchCity.cityName, searchCity.unit);

                if (weather.id > 0)
                {
                    historyModel.Type = "Ok";
                    historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

                    return Ok(weather);
                }
                else
                {

                    historyModel.Type = "BadRequest";
                    historyModel.Response = null;

                    return BadRequest();
                }
            }
            catch(InvalidOperationException)
            {
                historyModel.Type = "BadRequest";
                historyModel.Response = null;

                return BadRequest("Select unit");
            }
            finally
            {
                hIstoryRepository.AddHistory(historyModel);
            }
        }

        [Route("api/Weathers/ByLatLon")]
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByLonLat([FromBody] SearchLatLon searchLatLon)
        {
            var request = Request.Headers.Authorization; 

            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = AuthenticateService.GetUsernameFromJWT(request.ToString());
            historyModel.SearchRequest = "Lat/Lon";
            historyModel.Data = searchLatLon.Latitude.ToString() + ", " + searchLatLon.Longitude.ToString();

            try
            {
                var weather = await weatherServices.GetWeatherByLonLat(searchLatLon.Latitude, searchLatLon.Longitude, searchLatLon.unit);

                historyModel.Type = "Ok";
                historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

                return Ok(weather);
            }
            catch (InvalidOperationException)
            {
                historyModel.Type = "BadRequest";
                historyModel.Response = null;

                return BadRequest("Select unit");
            }
            finally
            {
                hIstoryRepository.AddHistory(historyModel);
            }
        }

        [Route("api/Weathers/ByCityId")]
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByCityId([FromBody] SearchCityId searchCityId)
        {
            var request = Request.Headers.Authorization;


            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = AuthenticateService.GetUsernameFromJWT(request.ToString());
            historyModel.SearchRequest = "CityId";
            historyModel.Data = searchCityId.CityId.ToString();

            try
            {
                var weather = await weatherServices.GetWeatherByCityId(searchCityId.CityId);

                if (weather.id > 0)
                {
                    historyModel.Type = "Ok";
                    historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

                    return Ok(weather);
                }
                else
                {
                    string message = "The cityId is incorrect";

                    historyModel.Type = "BadRequest";
                    historyModel.Response = null;

                    return BadRequest(message);
                }
            }
            catch (InvalidOperationException)
            {
                historyModel.Type = "BadRequest";
                historyModel.Response = null;

                return BadRequest("Select unit");
            }
            finally
            {
                hIstoryRepository.AddHistory(historyModel);
            }
        }
    }
}
