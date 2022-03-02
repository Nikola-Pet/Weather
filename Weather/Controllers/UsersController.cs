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
using Weather.Repository;

namespace Weather.Controllers
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
