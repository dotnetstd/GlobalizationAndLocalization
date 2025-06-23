using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Globalization;

namespace LocalizationRouting.Filters
{
    public class CultureRedirectFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.RouteData.Values.ContainsKey("culture") || String.IsNullOrWhiteSpace((string)context.RouteData.Values["culture"]))
            {
                var routeValues = context.RouteData.Values;
                routeValues["culture"] = CultureInfo.CurrentCulture.Name;
                context.Result = new RedirectToRouteResult(routeValues);
            }
        }
    }
}