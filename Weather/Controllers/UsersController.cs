using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Configuration;
using Weather.Models;
using LinqToDB.Data;
using Weather.Services;
using System.Web;
using System.Net.Http.Headers;

namespace Weather.Controllers
{
    public class UsersController : ApiController
    {
        private UserSevices userServices;
        private AuthenticateService authenticateService;

        public UsersController()
        {
            this.userServices = new UserSevices();
            this.authenticateService = new AuthenticateService();
        }


        // POST: api/LogIn
        public IHttpActionResult LogIn(string username, string password)
        {
            var user = userServices.GetUser(username, password);
            string token = authenticateService.GenerateToken(user.UserId, user.Username);

            HttpContext httpContext = HttpContext.Current;
           

            httpContext.Response.Headers.Add("Authorization", token);

            

           

            return Ok<string>(token);
        }

     



    }
}
