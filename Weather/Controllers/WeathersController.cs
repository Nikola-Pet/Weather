using System.Threading.Tasks;
using System.Web.Http;
using Orion.WeatherApi.Models;
using Orion.WeatherApi.Services;

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
        
        public async Task<WeatherModel> GetWeather(string city)
        {
            return await this.weatherServices.GetWeather(city);
        }
    }
}
