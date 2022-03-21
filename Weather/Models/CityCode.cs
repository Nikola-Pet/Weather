using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orion.WeatherApi.Models
{
    public class CityCode
    {
        public int CityId { get; set; }
        public int WeatherCityId { get; set; }
        public string Name { get; set; }    
    }
}