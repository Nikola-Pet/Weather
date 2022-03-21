using Newtonsoft.Json;
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

        public string QueryString(string data, string unit, string searchType, string controller)
        {
            string key = "631b2522cc672480a232e53d72c18a6c";
            string queryString = null;

            switch (searchType)
            {
                case "SearchByCity":
                     data = "q=" + data;
                    break;

                case "SearchByLatLon":
                    string[] LatLon = data.Split(' ');
                    
                    data = "lat=" + LatLon[0] + "&lon=" + LatLon[1];
                    break;

                case "SearchByCityId":
                    data = "id=" + data;
                    break;

            }


            switch (controller)
            {
                case "Weather":
                    queryString = "http://api.openweathermap.org/data/2.5/weather?"
                        + data + "&APPID=" + key;
                    break;

                case "Forecast":
                    queryString = "https://api.openweathermap.org/data/2.5/onecall?" + data + "&exclude=daily" + "&APPID=" + key;
                    break;
            }

            

            switch (unit)
            {
                case "Celsius":
                    return queryString += "&units=metric";

                case "Fahrenheit":
                    return queryString += "&units=imperial";

                case "Kelvin":
                    return queryString += "";
            }

            return null;
        }

        public async Task<string> JsonResults(string data, string unit, string searchType, string controller)
        {
            string queryString = QueryString(data, unit, searchType, controller);


            string results = await GetDataFromService(queryString).ConfigureAwait(false);

            return results;
        }
        

        public async Task<ResponseWeather> GetWeatherByCityName(string city, string unit)
        {
            
            

            string results = await JsonResults(city, unit, "SearchByCity", "Weather");

            ResponseWeather weather = JsonConvert.DeserializeObject<ResponseWeather>(results);

            return weather;
        }


        public async Task<ResponseWeather> GetWeatherByLonLat(double lat, double lon, string unit)
        {
            string a = lat.ToString();
            string b = lon.ToString();

            string data = a +" "+  b;


            string results = await JsonResults(data, unit, "SearchByLatLon", "Weather");

            ResponseWeather weather = JsonConvert.DeserializeObject<ResponseWeather>(results);

            return weather;
        }

        public async Task<ResponseWeather> GetWeatherByCityId(int cityId, string unit)
        {
            string results = await JsonResults(cityId.ToString(), unit, "SearchByCityId", "Weather");

            ResponseWeather weather = JsonConvert.DeserializeObject<ResponseWeather>(results);

            return weather;
        }
    }
}