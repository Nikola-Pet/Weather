

namespace Orion.WeatherApi.Models
{
    public class HystoryModel
    {
        public int HistoryID { get; set; }
        public string Username { get; set; }
        public string SearchRequest { get; set; }
        public string Data { get; set; }

        public string Type { get; set; }
        public string Response { get; set; }


    }
}