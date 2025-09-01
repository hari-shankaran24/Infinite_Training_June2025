using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Question2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable attribute routing
            config.MapHttpAttributeRoutes();

            // Other routes
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

}
