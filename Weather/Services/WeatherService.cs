﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Orion.WeatherApi.Models;

namespace Orion.WeatherApi.Services
{
    public class WeatherService
    {
        public async Task<string> GetDataFromService(string queryString)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(queryString);


            string json = null;
            if (response != null)
            {
                json = response.Content.ReadAsStringAsync().Result;
            }

            return json;
        }

        public async Task<ResponseWeather> GetWeatherByCityName(string city)
        {
            string key = "631b2522cc672480a232e53d72c18a6c";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?q="
                + city + "&APPID=" + key+ "&units=metric"; //kmh

            string results = await GetDataFromService(queryString).ConfigureAwait(false);

            ResponseWeather weather = JsonConvert.DeserializeObject<ResponseWeather>(results);

            return weather;
        }


        public async Task<ResponseWeather> GetWeatherByLonLat(double lat, double lon)
        {
            string key = "631b2522cc672480a232e53d72c18a6c";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?lat="+ lat + "&lon="
                + lon + "&APPID=" + key + "&units=metric"; 

            string results = await GetDataFromService(queryString).ConfigureAwait(false);

            ResponseWeather weather = JsonConvert.DeserializeObject<ResponseWeather>(results);

            return weather;
        }

        public async Task<ResponseWeather> GetWeatherByCityId(int cityId)
        {
            string key = "631b2522cc672480a232e53d72c18a6c";
            string queryString = "http://api.openweathermap.org/data/2.5/weather?id=" + cityId + "&APPID=" + key + "&units=metric"; 

            string results = await GetDataFromService(queryString).ConfigureAwait(false);

            ResponseWeather weather = JsonConvert.DeserializeObject<ResponseWeather>(results);

            return weather;
        }
    }
}