using System.Web.Http;
using System.Web;
using Orion.WeatherApi.Services;

using Orion.WeatherApi.Repository;

namespace Orion.WeatherApi.Controllers
{
    public class UsersController : ApiController
    {
        private IUserSql userSql;
        private AuthenticateService authenticateService;

        public UsersController()
        {
            this.authenticateService = new AuthenticateService();
            this.userSql = new UserSql();
        }


        // POST: api/LogIn
        public IHttpActionResult LogIn(string username, string password)
        {
            var user = userSql.GetUser(username, password);
            string token = authenticateService.GenerateToken(user.UserId, user.Username);

            HttpContext httpContext = HttpContext.Current;
           

            httpContext.Response.Headers.Add("Authorization", token);

            return Ok<string>(token);
        }
    }
}
