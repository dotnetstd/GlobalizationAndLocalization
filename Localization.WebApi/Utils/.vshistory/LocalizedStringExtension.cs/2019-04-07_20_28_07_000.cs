using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Localization.WebApi.Utils
{
    public static class LocalizedStringExtension
    {
        public static async Task<string> ToJsonStringAsync(this IEnumerable<LocalizedString> source, bool isCamelLowerCaseForKey, string prefixKey = "")
        {
            string json = string.Empty;
            var dicsTask = localizedStrsToDictionary(source);
            JsonSerializerSettings camelCaseFormatter = null;

            if (isCamelLowerCaseForKey)
            {
                camelCaseFormatter = new JsonSerializerSettings();
                camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            var dics = await dicsTask;
            json = camelCaseFormatter == null ?
                JsonConvert.SerializeObject(dics) :
                JsonConvert.SerializeObject(dics, camelCaseFormatter);

            if (!string.IsNullOrEmpty(prefixKey))
            {
                json = string.Concat("{\"", prefixKey, "\":", json, "}");
            }

            return json;
        }

        private static async Task<Dictionary<string, string>> localizedStrsToDictionary(IEnumerable<LocalizedString> localizedStrs)
        {
            IEnumerable<KeyValuePair<string, string>> values = localizedStrs.Select(x => new KeyValuePair<string, string>(
                 x.Name, x.Value
                ));
            Dictionary<string, string> dictionary = values.ToDictionary(k => k.Key, v => v.Value);
            return dictionary;
        }
    }
}
