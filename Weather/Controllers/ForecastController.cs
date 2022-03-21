using Newtonsoft.Json;
using Orion.WeatherApi.DTO;
using Orion.WeatherApi.Models;
using Orion.WeatherApi.Repository;
using Orion.WeatherApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Orion.WeatherApi.Controllers
{
    public class ForecastController : ApiController
    {
        private WeatherService weatherServices;
        private IHistoryRepository hIstoryRepository;
        private IpAddressService ipAddressService;

        public ForecastController()
        {
            this.weatherServices = new WeatherService();
            this.hIstoryRepository = new HistoryRepository();
            this.ipAddressService = new IpAddressService();
        }

        [Route("api/Forecast/SearchByCity")]
        [HttpPost]
        public async Task<IHttpActionResult> GetForecastByCityName([FromBody] SearchCity searchCity)
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
                    historyModel.TypeId = "Ok";
                    historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

                    return Ok(weather);
                }
                else
                {

                    historyModel.TypeId = "BadRequest";
                    historyModel.Response = null;

                    return BadRequest("City name is incorect");
                }
            }
            catch (InvalidOperationException)
            {
                historyModel.TypeId = "BadRequest";
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
