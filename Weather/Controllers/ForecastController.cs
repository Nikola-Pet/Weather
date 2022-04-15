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
using static Orion.WeatherApi.DTO.Enums.Response;

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
            historyModel.Request = "ByCity";
            historyModel.Data = searchLatlon.Latitude.ToString()+","+searchLatlon.Longitude.ToString();

            try
            {
                var forecast = await weatherServices.GetForecastByLatLon(searchLatlon.Latitude, searchLatlon.Longitude, searchLatlon.unit.ToString());


                if (forecast != null)
                {
                    historyModel.TypeId = ResponseType.Ok;
                    historyModel.Response = JsonConvert.SerializeObject(forecast).ToString();

                    return Ok(forecast);
                }
                else
                {

                    historyModel.TypeId = ResponseType.BadRequest;
                    historyModel.Response = null;

                    return BadRequest("City name is incorect");
                }
            }
            catch (InvalidOperationException)
            {
                historyModel.TypeId = ResponseType.BadRequest;
                historyModel.Response = null;

                return BadRequest("Select unit: C, F or K");
            }
            finally
            {
                hIstoryRepository.AddHistory(historyModel);
            }
        }
    }
}
