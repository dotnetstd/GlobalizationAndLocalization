using System;

namespace My.Extensions.Localization.ReportMissingKeys.Models
{
    public class MissingResourceKey
    {
        public string Key { get; set; }

        public string ResourceName { get; set; }

        public string Assembly { get; set; }

        public string[] Cultures { get; set; }
    }
}