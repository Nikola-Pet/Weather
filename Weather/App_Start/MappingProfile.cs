using AutoMapper;
using Orion.WeatherApi.DTO;
using Orion.WeatherApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CityCode, CityCodeList>();
            
            var weatherMap = CreateMap<ResponseWeather, WeatherView>();
            weatherMap.ForMember(x => x.Temperature, y => y.MapFrom(z => z.main.temp));
            weatherMap.ForMember(x => x.Wind, y => y.MapFrom(z => z.wind.speed));
            weatherMap.ForMember(x => x.Humidity, y => y.MapFrom(z => z.main.humidity));
            weatherMap.ForMember(x => x.Sunrise, y => y.MapFrom(z => DateConverter.ConvertDate(z.sys.sunrise)));
            weatherMap.ForMember(x => x.Sunset, y => y.MapFrom(z => DateConverter.ConvertDate(z.sys.sunset)));
        }
    }

    public class DateConverter
    { 
        public static DateTime ConvertDate (double unix)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(unix).ToLocalTime();
            return dt;
        }
    }

}