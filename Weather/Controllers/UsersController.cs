using System.Web.Http;
using System.Web;
using Orion.WeatherApi.Services;

using Orion.WeatherApi.Repository;
using Orion.WeatherApi.DTO;
using System.Net.Http;
using System.Net;
using System;
using Orion.WeatherApi.Models;
using static Orion.WeatherApi.DTO.Enums.Response;

namespace Orion.WeatherApi.Controllers
{
    public class UsersController : ApiController
    {
        private IUserRepository userRepository;
        private IHistoryRepository historyRepository;
        private IpAddressService ipAddressService;

        public UsersController()
        {
            this.userRepository = new UserRepository();
            this.historyRepository = new HistoryRepository();
            this.ipAddressService = new IpAddressService(); 
        }


        // POST: api/Users
        [HttpGet]
        public  IHttpActionResult LogIn([FromBody] LogInRequest logInRequest)
        {
            var user = userRepository.GetUser(logInRequest.Username, Utility.Encrypt(logInRequest.Password));

            if (user.UserId == 0)
            {
                HystoryModel history = new HystoryModel();
                //history.Username = null;
                history.IPAddress = ipAddressService.GetIp();
                history.Request = "LogIn";
                history.Data = logInRequest.Username + " " + logInRequest.Password;
                history.TypeId = ResponseType.Unauthorized;

                historyRepository.AddHistory(history);


                return BadRequest("User name or password is invalid");
            }

            return Ok(AuthenticateService.GenerateToken(user.UserId, user.Username));
        }
    }
}
