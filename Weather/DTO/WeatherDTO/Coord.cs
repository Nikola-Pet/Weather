using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orion.WeatherApi.DTO.WeatherDTO
{
    public class Coord
    {
        [JsonProperty("lon")]
        public double Longitude { get; set; }

        [JsonProperty("lan")]
        public double Latitude { get; set; }
    }
}