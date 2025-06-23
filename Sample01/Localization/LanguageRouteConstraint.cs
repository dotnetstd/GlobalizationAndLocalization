using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MainApp.Models.Localization
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (!values.ContainsKey("lang")) return false;
            var lang = values["lang"].ToString();
            return lang == "en" || lang == "fa" || lang == "fr";
        }
    }
}