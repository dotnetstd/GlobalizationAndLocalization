using MainApp.Models.Localization;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.IO;

namespace MainApp.Languages
{
    public static class en_US
    {
        public static List<Resource> GetList()
        {
            // Returns "Welcome" for en-US and "خوش آمدید" for fa-IR
            //var welcome = Startup._e["Welcome"];
            //<h1> @Startup._e["Welcome"] </h1>

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<List<Resource>>(File.ReadAllText("Languages/en-US.json"), jsonSerializerSettings);
        }
    }
}
