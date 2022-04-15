using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orion.WeatherApi.DTO.Enums
{
    public class Response
    {
        public enum ResponseType
        { 
            Ok,
            BadRequest,
            Unauthorized
        }
    }
}