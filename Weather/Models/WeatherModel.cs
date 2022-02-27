using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.Models
{
    public class WeatherModel
    {
        public string Title { get; set; } = " ";
        public string Temperature { get; set; } = " ";
        public string Wind { get; set; } = " ";
        public string Humidity { get; set; } = " ";
        public string Visibility { get; set; } = " ";
        public string Sunrise { get; set; } = " ";
        public string Sunset { get; set; } = " ";
    }
}