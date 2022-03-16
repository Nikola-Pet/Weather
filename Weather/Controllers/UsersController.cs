using System.Web.Http;
using System.Web;
using Orion.WeatherApi.Services;

using Orion.WeatherApi.Repository;
using Orion.WeatherApi.DTO;
using System.Net.Http;
using System.Net;

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


        // POST: api/Users
        [HttpGet]
        public HttpResponseMessage LogIn([FromBody] LogInRequest logInRequest)
        {
            var user = userSql.GetUser(logInRequest.Username, logInRequest.Password);
            if (user != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, AuthenticateService.GenerateToken(user.UserId, user.Username));
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadGateway, "User name or password is invalid");
            }
        }
    }
}
