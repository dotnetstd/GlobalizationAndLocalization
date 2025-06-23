using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using My.Extensions.Localization.ReportMissingKeys.Models;

namespace My.Extensions.Localization.ReportMissingKeys.Interfaces
{
    public interface IOutputFormatter
    {
        Task WriteAsync(Stream stream, IEnumerable<MissingResourceKey> missingResources);

        string ContentTypeProduced { get; }
    }
}