﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Weather.JWT;
using Weather.Repository;

namespace Weather
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //services.AddScoped<IUserSql, UserSql>();

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new ValidateTokenHandelr());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserSql, UserSql>();
        }
    }
}
