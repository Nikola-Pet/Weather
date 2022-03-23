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
using AutoMapper;
using System.Collections.Generic;

namespace Orion.WeatherApi.Controllers
{
    [CustomAuthenticationFilter]
    public class WeathersController : ApiController
    {
        private readonly IMapper _mapper;

        private WeatherService weatherServices;
        private IHistoryRepository hIstoryRepository;
        private IpAddressService ipAddressService;
        private ICityCodeRepository cityCodeRepository;

        public WeathersController(IMapper mapper)
        {
            _mapper = mapper;

            this.weatherServices = new WeatherService();
            this.hIstoryRepository = new HistoryRepository();
            this.ipAddressService = new IpAddressService();
            this.cityCodeRepository = new CityCodeRepository();
        }
       
        [Route("api/Weathers/SearchByCity")]
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
                    historyModel.TypeId = "Ok";
                    historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

                    return Ok(_mapper.Map<WeatherView>(weather));
                }
                else
                {

                    historyModel.TypeId = "BadRequest";
                    historyModel.Response = null;

                    return BadRequest("City name is incorect");
                }
            }
            catch(InvalidOperationException)
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

        [Route("api/Weathers/SearchByLatLon")]
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByLonLat([FromBody] SearchLatLon searchLatLon)
        {
            var request = Request.Headers.Authorization; 

            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = AuthenticateService.GetUsernameFromJWT(request.ToString());
            historyModel.IPAddress = ipAddressService.GetIp();
            historyModel.SearchRequest = "Lat/Lon";
            historyModel.Data = searchLatLon.Latitude.ToString() + ", " + searchLatLon.Longitude.ToString();

            try
            {
                var weather = await weatherServices.GetWeatherByLonLat(searchLatLon.Latitude, searchLatLon.Longitude, searchLatLon.unit);

                historyModel.TypeId = "Ok";
                historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

                return Ok(weather);
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

        [Route("api/Weathers/SearchByCityId")]
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByCityId([FromBody] SearchCityId searchCityId)
        {
            var request = Request.Headers.Authorization;


            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = AuthenticateService.GetUsernameFromJWT(request.ToString());
            historyModel.IPAddress = ipAddressService.GetIp();
            historyModel.SearchRequest = "CityId";
            historyModel.Data = searchCityId.CityId.ToString();

            try
            {
                int id = cityCodeRepository.GetWeatherCityId(searchCityId.CityId);

                if (id == 0)
                {
                    string message = "The cityId is incorrect";

                    historyModel.TypeId = "BadRequest";
                    historyModel.Response = null;

                    return BadRequest(message);
                }

                var weather = await weatherServices.GetWeatherByCityId(id, searchCityId.unit);

                if (weather.id > 0)
                {
                    historyModel.TypeId = "Ok";
                    historyModel.Response = JsonConvert.SerializeObject(weather).ToString();

                    return Ok(weather);
                }
                else
                {
                    string message = "The cityId is incorrect";

                    historyModel.TypeId = "BadRequest";
                    historyModel.Response = null;

                    return BadRequest(message);
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

        [Route("api/Weathers/GetCityCodes")]
        [HttpGet]
        public IHttpActionResult GetCityCodes()
        {
            var cityCodes =  cityCodeRepository.GetAllCityCode();

            return  Ok(_mapper.Map<IEnumerable<CityCodeList>>(cityCodes));
        }
    }
}
