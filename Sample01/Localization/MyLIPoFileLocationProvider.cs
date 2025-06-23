using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

using OrchardCore.Localization;

using System.Collections.Generic;
using System.IO;

namespace Sample01.Localization
{
	public class MyLIPoFileLocationProvider : ILocalizationFileLocationProvider
	{
		private readonly string _resourcesContainer;

		public MyLIPoFileLocationProvider(IOptions<LocalizationOptions> localizationOptions)
		{
			_resourcesContainer = localizationOptions.Value.ResourcesPath;
		}

		public IEnumerable<IFileInfo> GetLocations(string cultureName)
		{
			yield return new PhysicalFileInfo(new FileInfo("Localization"));
		}
	}
}
