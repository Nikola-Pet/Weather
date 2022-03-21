using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.WeatherApi.Repository
{
    public interface IFiledLoginRepository
    {
        void AddFiledLogin(string username, string password);
    }
}
