//using Microsoft.AspNetCore.Routing;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LocalizerCustom.Models
//{
//    public class LanguageRouteConstraint : IRouteConstraint
//    {
//        private readonly AppLanguages _languageSettings;

//        public LanguageRouteConstraint(IHostingEnvironment hostingEnvironment)
//        {
//            var builder = new ConfigurationBuilder()
//                .SetBasePath(hostingEnvironment.ContentRootPath)
//                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//            IConfigurationRoot configuration = builder.Build();

//            _languageSettings = new AppLanguages();
//            configuration.GetSection("AppLanguages").Bind(_languageSettings);
//        }

//        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
//        {
//            if (!values.ContainsKey("lang"))
//            {
//                return false;
//            }

//            var lang = values["lang"].ToString();
//            foreach (Language lang_in_app in _languageSettings.Dict.Values)
//            {
//                if (lang == lang_in_app.Icc)
//                {
//                    return true;
//                }
//            }
//            return false;
//        }
//    }
//}
