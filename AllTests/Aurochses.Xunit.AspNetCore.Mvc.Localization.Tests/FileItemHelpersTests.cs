using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class FileItemHelpersTests
    {
        [Fact]
        public void GetFileItems_Success()
        {
            // Arrange
            var projectPath = AppDomain.CurrentDomain.BaseDirectory;

            var list = new List<FileItem>
            {
                new FileItem(projectPath, @"\Fakes\FileItemHelpers\GetFileItems", "", "1.txt"),
                new FileItem(projectPath, @"\Fakes\FileItemHelpers\GetFileItems", @"\SubFolder", "2.txt")
            };

            // Act
            var fileItems = FileItemHelpers.GetFileItems(projectPath, @"\Fakes\FileItemHelpers\GetFileItems", "", "*.txt");

            // Assert
            Assert.Equal(2, fileItems.Count);
            foreach (var item in list)
            {
                Assert.NotNull(fileItems.FirstOrDefault(x => x.GetFullPath() == item.GetFullPath()));
            }
        }
    }
}