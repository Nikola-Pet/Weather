using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Orion.WeatherApi.Models;

namespace Orion.WeatherApi.Services
{
    public class WeatherServices
    {
        public  async Task<dynamic> GetDataFromService(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);

            

            dynamic data = null;
            if (response != null)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject(json);
            }

            return data;
        }

        public  async Task<WeatherModel> GetWeather(string city)
        {
            string key = "631b2522cc672480a232e53d72c18a6c";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?q="
                + city + ",uk&APPID=" + key;

            dynamic results = await GetDataFromService(queryString).ConfigureAwait(false);

            if (results["weather"] != null)
            {
                WeatherModel weather = new WeatherModel();
                weather.Title = (string)results["name"];
                weather.Temperature = (string)results["main"]["temp"] + " F";
                weather.Wind = (string)results["wind"]["speed"] + " mph";
                weather.Humidity = (string)results["main"]["humidity"] + " %";
                weather.Visibility = (string)results["weather"][0]["main"];

                DateTime time = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                DateTime sunrise = time.AddSeconds((double)results["sys"]["sunrise"]);
                DateTime sunset = time.AddSeconds((double)results["sys"]["sunset"]);
                weather.Sunrise = sunrise.ToString() + " UTC";
                weather.Sunset = sunset.ToString() + " UTC";
                return weather;
            }
            else
            {
                return null;
            }
        }
    }
}