using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Localization.DotNetTips.Models
{
    public class LanguageRouteConstraint : IRouteConstraint
    {
        public bool Match(
            HttpContext httpContext,
            IRouter route,
            string routeKey,
            RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            if (!values.ContainsKey("lang"))
            {
                return false;
            }
            var lang = values["lang"].ToString();
            var result = lang == "fa" || lang == "en";
            return result;
        }
    }
}
