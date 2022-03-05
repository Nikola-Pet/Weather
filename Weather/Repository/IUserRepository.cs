using System;
using Orion.WeatherApi.Models;

namespace Orion.WeatherApi.Repository
{
    public interface IUserRepository 
    {
        UserModel GetUser(string username, string password);
    }
}
