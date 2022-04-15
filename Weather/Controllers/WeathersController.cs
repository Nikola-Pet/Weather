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
using static Orion.WeatherApi.DTO.Enums.Response;
using static Orion.WeatherApi.DTO.Enums.Unit;

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
            historyModel.Request = "ByCity";
            historyModel.Data = searchCity.cityName;

            bool successUnit = false;
            bool successCity = false;

            try
            {
                successUnit = Enum.IsDefined(typeof(UnitType), searchCity.unit);
                successCity = cityCodeRepository.ValidateCity(searchCity.cityName);

                var weather = await weatherServices.GetWeatherByCityName(searchCity.cityName, searchCity.unit.ToString());

                if (successCity == true)
                {
                    historyModel.TypeId = ResponseType.Ok;
                    historyModel.Response = JsonConvert.SerializeObject(_mapper.Map<WeatherView>(weather)).ToString();

                    return Ok(_mapper.Map<WeatherView>(weather));
                }
                else
                {

                    historyModel.TypeId = ResponseType.BadRequest;
                    historyModel.Response = null;

                    return BadRequest("City name is incorect");
                }
            }
            catch(InvalidOperationException) when(successUnit == false)
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

        [Route("api/Weathers/SearchByLatLon")]
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByLonLat([FromBody] SearchLatLon searchLatLon)
        {
            var request = Request.Headers.Authorization;
            bool success = false;

            

            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = AuthenticateService.GetUsernameFromJWT(request.ToString());
            historyModel.IPAddress = ipAddressService.GetIp();
            historyModel.Request = "Lat/Lon";
            historyModel.Data = searchLatLon.Latitude.ToString() + ", " + searchLatLon.Longitude.ToString();

            if ((searchLatLon.Longitude > 180 || searchLatLon.Longitude < -180) || (searchLatLon.Latitude > 90 || searchLatLon.Latitude < -90))
            {
                historyModel.TypeId = ResponseType.BadRequest;
                historyModel.Response = null;

                return BadRequest("Check the entered parameters. Longitude(-180,180). Latitude(-90,90)");
            }

            try
            {
                success = Enum.IsDefined(typeof(UnitType), searchLatLon.unit);

                var weather = await weatherServices.GetWeatherByLonLat(searchLatLon.Latitude, searchLatLon.Longitude, searchLatLon.unit.ToString());

                historyModel.TypeId = ResponseType.Ok;
                historyModel.Response = JsonConvert.SerializeObject(_mapper.Map<WeatherView>(weather)).ToString();

                return Ok(_mapper.Map<WeatherView>(weather));
            }
            catch (InvalidOperationException) when (success == false)
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

        [Route("api/Weathers/SearchByCityId")]
        [HttpPost]
        public async Task<IHttpActionResult> GetWeatherByCityId([FromBody] SearchCityId searchCityId)
        {
            var request = Request.Headers.Authorization;
            bool success = false;

            HystoryModel historyModel = new HystoryModel();
            historyModel.Username = AuthenticateService.GetUsernameFromJWT(request.ToString());
            historyModel.IPAddress = ipAddressService.GetIp();
            historyModel.Request = "CityId";
            historyModel.Data = searchCityId.CityId.ToString();

            try
            {
                int id = cityCodeRepository.GetWeatherCityId(searchCityId.CityId);
                success = Enum.IsDefined(typeof(UnitType), searchCityId.unit);


                if (id == 0)
                {
                    string message = "The cityId is incorrect";

                    historyModel.TypeId = ResponseType.BadRequest; 
                    historyModel.Response = null;

                    return BadRequest(message);
                }

                var weather = await weatherServices.GetWeatherByCityId(id, searchCityId.unit.ToString());

               
                    historyModel.TypeId = ResponseType.Ok;
                    historyModel.Response = JsonConvert.SerializeObject(_mapper.Map<WeatherView>(weather)).ToString();

                    return Ok(_mapper.Map<WeatherView>(weather));
               
            }
            catch (InvalidOperationException) when (success == false)
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

        [Route("api/Weathers/GetCityCodes")]
        [HttpGet]
        public IHttpActionResult GetCityCodes()
        {
            var cityCodes =  cityCodeRepository.GetAllCityCode();

            return  Ok(_mapper.Map<IEnumerable<CityCodeList>>(cityCodes));
        }
    }
}
