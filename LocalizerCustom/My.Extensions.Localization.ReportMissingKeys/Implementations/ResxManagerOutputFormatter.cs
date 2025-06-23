using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using My.Extensions.Localization.ReportMissingKeys.Interfaces;
using My.Extensions.Localization.ReportMissingKeys.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace My.Extensions.Localization.ReportMissingKeys.Implementations
{
    class ResxManagerOutputFormatter : IOutputFormatter
    {
        public string ContentTypeProduced => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public Task WriteAsync(Stream stream, IEnumerable<MissingResourceKey> missingResources)
        {
            var cultures = missingResources.SelectMany(r => r.Cultures).Distinct();

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
            {
                // Add a WorkbookPart to the document.
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                // Add a WorksheetPart to the WorkbookPart.
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();
                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "ResXResourceManager"
                };
                sheets.Append(sheet);

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                var row = new Row();
                row.Append(
                    new Cell { CellValue = new CellValue("Project"), DataType = new EnumValue<CellValues>(CellValues.String) },
                    new Cell { CellValue = new CellValue("File"), DataType = new EnumValue<CellValues>(CellValues.String) },
                    new Cell { CellValue = new CellValue("Key"), DataType = new EnumValue<CellValues>(CellValues.String) }
                );
                foreach(var culture in cultures) {
                    row.Append(
                        new Cell { CellValue = new CellValue($"Comment.{ culture }"), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell { CellValue = new CellValue($".{ culture }"), DataType = new EnumValue<CellValues>(CellValues.String) }
                    );
                }
                sheetData.AppendChild(row);
                
                foreach (var key in missingResources)
                {
                    row = new Row();

                    row.Append(
                        new Cell { CellValue = new CellValue(key.Assembly), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell { CellValue = new CellValue(key.ResourceName.Replace('.', '\\')), DataType = new EnumValue<CellValues>(CellValues.String) },
                        new Cell { CellValue = new CellValue(key.Key), DataType = new EnumValue<CellValues>(CellValues.String) }
                    );
                    foreach (var culture in cultures)
                    {
                        row.Append(
                            new Cell { CellValue = new CellValue(""), DataType = new EnumValue<CellValues>(CellValues.String) },
                            // We should not overwrite already existing culture variants of a key
                            new Cell { CellValue = new CellValue(key.Cultures.Contains(culture) ? "?" : ""), DataType = new EnumValue<CellValues>(CellValues.String) }
                        );
                    }
                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
                document.Save();
            }

            return Task.CompletedTask;
        }
    }
}