
using static Orion.WeatherApi.DTO.Enums.Unit;

namespace Orion.WeatherApi.DTO
{
    public class SearchCity
    {
        public string cityName { get; set; }
        public string unit { get; set; }
    }
}