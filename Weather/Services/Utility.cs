using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Orion.WeatherApi.Services
{
    public static class Utility
    {
        public static string Key = "s4!m8p1eqx@y";

        public static string Encrypt(string password)
        {
            password += Key;
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }
    }
}