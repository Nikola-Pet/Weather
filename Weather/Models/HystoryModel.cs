

using System;

namespace Orion.WeatherApi.Models
{
    public class HystoryModel
    {
        public int HistoryID { get; set; }
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string SearchRequest { get; set; }
        public string Data { get; set; }
        public string TypeId { get; set; }
        public string Response { get; set; }
    }
}