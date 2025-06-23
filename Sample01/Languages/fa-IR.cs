using System.Collections.Generic;
using System.IO;
using System.Text;

using MainApp.Models.Localization;

using Newtonsoft.Json;

namespace MainApp.Languages
{
    public static class fa_IR
    {
        public static List<Resource> GetList()
        {
            var jsonSerializerSettings = new JsonSerializerSettings {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            return JsonConvert.DeserializeObject<List<Resource>>(File.ReadAllText("Languages/fa-IR.json", Encoding.UTF8), jsonSerializerSettings);
        }
    }
}
