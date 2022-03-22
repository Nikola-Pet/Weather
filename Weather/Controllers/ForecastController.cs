using Newtonsoft.Json;
using Orion.WeatherApi.DTO;
using Orion.WeatherApi.JWT;
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
    [CustomAuthenticationFilter]
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

        [Route("api/Forecast/SearchByLatLon")]
        [HttpPost]
        public async Task<IHttpActionResult> GetForecastByCityName([FromBody] SearchLatLon searchLatlon)
        {
            var request = Request.Headers.Authorization;

            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = AuthenticateService.GetUsernameFromJWT(request.ToString());
            historyModel.IPAddress = ipAddressService.GetIp();
            historyModel.SearchRequest = "ByCity";
            historyModel.Data = searchLatlon.Latitude.ToString()+","+searchLatlon.Longitude.ToString();

            try
            {
                var forecast = await weatherServices.GetForecastByLatLon(searchLatlon.Latitude, searchLatlon.Longitude, searchLatlon.unit);


                if (forecast != null)
                {
                    historyModel.TypeId = "Ok";
                    historyModel.Response = JsonConvert.SerializeObject(forecast).ToString();

                    return Ok(forecast);
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
