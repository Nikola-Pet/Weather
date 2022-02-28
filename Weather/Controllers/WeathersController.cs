using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Weather.Models;
using Weather.Services;

namespace Weather.Controllers
{
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
