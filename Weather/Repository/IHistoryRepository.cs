using Orion.WeatherApi.Models;


namespace Orion.WeatherApi.Repository
{
    public interface IHistoryRepository
    {
        void AddHistory(HystoryModel history);
    }
}
