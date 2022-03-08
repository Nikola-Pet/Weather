using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orion.WeatherApi.DTO
{
    public class LogInRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }    
    }
}