using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orion.WeatherApi.Services
{
    public class IpAddressService
    {
        public string GetIp()
        {

            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }
    }
}