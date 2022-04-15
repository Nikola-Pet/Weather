using System;
using static Orion.WeatherApi.DTO.Enums.Response;

namespace Orion.WeatherApi.Models
{
    public class HystoryModel
    {
        public int HistoryID { get; set; }
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Request { get; set; }
        public string Data { get; set; }
        public ResponseType TypeId { get; set; }
        public string Response { get; set; }
    }
}