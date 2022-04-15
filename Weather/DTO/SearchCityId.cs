
using static Orion.WeatherApi.DTO.Enums.Unit;

namespace Orion.WeatherApi.Controllers
{
    public class SearchCityId
    {
        public int CityId { get; set; }
        public string unit { get; set; }

    }
}