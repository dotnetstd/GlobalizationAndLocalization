using System.Collections.Generic;

namespace JsonLocalizer.AspNetCoreJsonLocalization
{
	public class JsonLocalizationFormat
	{
		public string Key { get; set; }
		public Dictionary<string, string> Value =
			new Dictionary<string, string>();
	}
}
