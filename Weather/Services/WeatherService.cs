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

        public string QueryString(string data, string unit, string searchType)
        {
            string key = "631b2522cc672480a232e53d72c18a6c";

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

            string queryString = "http://api.openweathermap.org/data/2.5/weather?"
                + data + "&APPID=" + key;

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
        

        public async Task<ResponseWeather> GetWeatherByCityName(string city, string unit)
        {
            
            string queryString = QueryString(city, unit, "SearchByCity");
            

            string results = await GetDataFromService(queryString).ConfigureAwait(false);

            ResponseWeather weather = JsonConvert.DeserializeObject<ResponseWeather>(results);

            return weather;
        }


        public async Task<ResponseWeather> GetWeatherByLonLat(double lat, double lon, string unit)
        {
            string a = lat.ToString();
            string b = lon.ToString();

            string data = a +" "+  b;

            string queryString = QueryString(data, unit, "SearchByLatLon");

            string results = await GetDataFromService(queryString).ConfigureAwait(false);

            ResponseWeather weather = JsonConvert.DeserializeObject<ResponseWeather>(results);

            return weather;
        }

        public async Task<ResponseWeather> GetWeatherByCityId(int cityId, string unit)
        {
            string queryString = QueryString(cityId.ToString(), unit, "SearchByCityId");

            string results = await GetDataFromService(queryString).ConfigureAwait(false);

            ResponseWeather weather = JsonConvert.DeserializeObject<ResponseWeather>(results);

            return weather;
        }
    }
}