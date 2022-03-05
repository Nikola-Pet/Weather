using System.Web.Http;
using System.Web;
using Orion.WeatherApi.Services;

using Orion.WeatherApi.Repository;
using Weather.DTO;

namespace Orion.WeatherApi.Controllers
{
    public class UsersController : ApiController
    {
        private IUserRepository userSql;
        private AuthenticateService authenticateService;

        public UsersController()
        {
            this.authenticateService = new AuthenticateService();
            this.userSql = new UserRepository();
        }


        // POST: api/LogIn
        public IHttpActionResult LogIn([FromBody] LogInRequest logInRequest)
        {
            var user = userSql.GetUser(logInRequest.Username, logInRequest.Password);
            string token = authenticateService.GenerateToken(user.UserId, user.Username);

            HttpContext httpContext = HttpContext.Current;
           

            httpContext.Response.Headers.Add("Authorization", token);

            return Ok<string>(token);
        }
    }
}
