using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using DbLocalizationProvider.Import;
using Localization.Xliff.OM.Core;
using Localization.Xliff.OM.Serialization;

namespace DbLocalizationProvider.Xliff
{
    public class FormatParser : IResourceFormatParser
    {
        private static readonly string[] _extensions = { ".xlf", ".xliff" };

        public string FormatName => "XLIFF v2.0";

        public string[] SupportedFileExtensions => _extensions;

        public string ProviderId => "xliff";

        public ParseResult Parse(string fileContent)
        {
            var reader = new XliffReader();
            var doc = reader.Deserialize(AsStream(fileContent));

            var result = new List<LocalizationResource>();
            var languages = new List<string>();

            foreach(var file in doc.Files)
            {
                foreach(var container in file.Containers.OfType<Unit>())
                {
                    foreach(var resource in container.Resources)
                    {
                        result.Add(new LocalizationResource(XmlConvert.DecodeName(resource.Id))
                                   {
                                       Translations = new List<LocalizationResourceTranslation>
                                                      {
                                                          new LocalizationResourceTranslation
                                                          {
                                                              Language = resource.Target.Language,
                                                              Value = resource.Target.Text.OfType<CDataTag>().FirstOrDefault()?.Text
                                                          }
                                                      }
                                   });

                        if(!languages.Contains(resource.Target.Language))
                            languages.Add(resource.Target.Language);
                    }
                }
            }

            return new ParseResult(result, languages.Select(l => new CultureInfo(l)).ToList());
        }

        private static Stream AsStream(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;

            return stream;
        }
    }
}
