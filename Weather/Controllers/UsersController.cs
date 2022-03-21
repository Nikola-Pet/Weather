using System.Web.Http;
using System.Web;
using Orion.WeatherApi.Services;

using Orion.WeatherApi.Repository;
using Orion.WeatherApi.DTO;
using System.Net.Http;
using System.Net;
using System;

namespace Orion.WeatherApi.Controllers
{
    public class UsersController : ApiController
    {
        private IUserRepository userRepository;
        private IFiledLoginRepository filedLoginRepository;

        public UsersController()
        {
            this.userRepository = new UserRepository();
            this.filedLoginRepository = new FiledLoginRepository();
        }


        // POST: api/Users
        [HttpGet]
        public  IHttpActionResult LogIn([FromBody] LogInRequest logInRequest)
        {
            var user = userRepository.GetUser(logInRequest.Username, Utility.Encrypt(logInRequest.Password));

            if (user.UserId == 0)
            {
                filedLoginRepository.AddFiledLogin(logInRequest.Username, logInRequest.Password);
                return BadRequest("User name or password is invalid");
            }

            return Ok(AuthenticateService.GenerateToken(user.UserId, user.Username));
        }
    }
}
