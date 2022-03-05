using System.Threading.Tasks;
using System.Web.Http;
using Orion.WeatherApi.Models;
using Orion.WeatherApi.Services;
using Weather.DTO;

namespace Orion.WeatherApi.Controllers
{
    [Authorize]
    public class WeathersController : ApiController
    {

        private WeatherServices weatherServices;

        public WeathersController()
        {
            this.weatherServices = new WeatherServices();   
        }
        
        [HttpPost]
        public async Task<WeatherModel> GetWeather([FromBody] SearchCity searchCity)
        {
            return await this.weatherServices.GetWeather(searchCity.cityName);
        }
    }
}
