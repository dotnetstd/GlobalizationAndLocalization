using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class ViewLocalizationAssertTests
    {
        private readonly string _projectPath;

        public ViewLocalizationAssertTests()
        {
            _projectPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        [Fact]
        public void Validate_Success()
        {
            // Arrange
            var list = new List<LocalizedFileItem>
            {
                new LocalizedFileItem(_projectPath, @"\Fakes\ViewLocalizationAssert\Validate\Views", "", "Index.cshtml")
                {
                    Names =
                    {
                        "TestName",
                        "SecondTestName"
                    }
                }
            };

            // Act
            var localizedFileItems = ViewLocalizationAssert.Validate(
                _projectPath,
                @"\Fakes\ViewLocalizationAssert\Validate\Resources\Views",
                @"\Fakes\ViewLocalizationAssert\Validate\Views"
            );

            // Assert
            Assert.Single(localizedFileItems);
            foreach (var item in list)
            {
                var localizedFileItem = localizedFileItems.FirstOrDefault(x => x.RelativePath == item.RelativePath && x.FileName == item.FileName);

                Assert.NotNull(localizedFileItem);
                Assert.Equal(item.Names, localizedFileItem.Names);
            }
        }
    }
}