using System.Threading.Tasks;
using System.Web.Http;
using Orion.WeatherApi.Models;
using Orion.WeatherApi.Repository;
using Orion.WeatherApi.Services;
using Orion.WeatherApi.DTO;
using System.Web;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace Orion.WeatherApi.Controllers
{
    [Authorize]
    public class WeathersController : ApiController
    {
        private AuthenticateService authenticateService;
        private WeatherServices weatherServices;
        private IHistoryRepository hIstoryRepository;

        public WeathersController()
        {
            this.weatherServices = new WeatherServices(); 
            this.hIstoryRepository = new HistoryRepository();
            this.authenticateService = new AuthenticateService();
        }

        // POST: api/Weathers
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByCity([FromBody] SearchCity searchCity, HttpRequestMessage request)
        {           
            string token = authenticateService.GetJwtTokenString(request);

            string username = authenticateService.ValidateUsernameJwtToken(token);

            var weather = await weatherServices.GetWeatherByCity(searchCity.cityName);

            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = username;
            historyModel.SearchRequest = "ByCity";
            historyModel.Data = searchCity.cityName;
            historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

            try
            {
                if (weather.name != null)
                {
                    historyModel.Type = "Ok";

                    return Ok(weather);
                }
                else
                {
                    historyModel.Type = "BadRequest";

                    return BadRequest("The city was not found or the city name is incorrect");
                }
            }
            finally
            {
                hIstoryRepository.AddHistory(historyModel);
            }


        }
    }
}
