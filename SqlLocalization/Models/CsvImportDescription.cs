using Microsoft.AspNetCore.Http;

using System.Collections.Generic;

namespace SqlLocalization.Models
{
    public class CsvImportDescription
    {
        public string Information { get; set; }
        public ICollection<IFormFile> File { get; set; }
    }
}