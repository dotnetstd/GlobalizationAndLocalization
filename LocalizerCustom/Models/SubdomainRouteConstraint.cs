using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using System;

namespace LocalizerCustom.Models
{
    public class SubdomainRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string url = httpContext.Request.Headers["HOST"];
            int index = url.IndexOf(".", StringComparison.Ordinal);

            if (index < 0)
                return false;

            string sub = url.Split('.')[0];
            if (sub != "tr" && sub != "en")
                return false;


            values["culture"] = sub;
            return true;
        }
    }
}
