using System;
using Orion.WeatherApi.Models;

namespace Orion.WeatherApi.Repository
{
    public interface IUserSql 
    {
        UserModel GetUser(string username, string password);
    }
}
