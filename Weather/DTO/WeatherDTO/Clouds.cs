using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orion.WeatherApi.DTO.WeatherDTO
{
    public class Clouds
    {
        [JsonProperty("all")]
        public int All { get; set; }

    }
}