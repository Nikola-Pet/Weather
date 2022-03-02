using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;
using Weather.Models;

namespace Weather.Repository
{
    public interface IUserSql 
    {
        UserModel GetUser(string username, string password);
    }
}
