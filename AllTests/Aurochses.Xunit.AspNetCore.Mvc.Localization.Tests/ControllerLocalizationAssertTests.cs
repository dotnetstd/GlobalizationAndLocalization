using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class ControllerLocalizationAssertTests
    {
        private readonly string _projectPath;

        public ControllerLocalizationAssertTests()
        {
            _projectPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        [Fact]
        public void Validate_Success()
        {
            // Arrange
            var list = new List<LocalizedFileItem>
            {
                new LocalizedFileItem(_projectPath, @"\Fakes\Controller\Validate\Controllers", "", "HomeController.cs")
                {
                    Names =
                    {
                        "TestName",
                        "SecondTestName",
                        "PredefinedTestName"
                    }
                }
            };

            var predefinedLocalizedFileItems = new List<LocalizedFileItem>
            {
                new LocalizedFileItem(_projectPath, @"\Fakes\Controller\Validate\Controllers", "", "HomeController.cs")
                {
                    Names =
                    {
                        "PredefinedTestName"
                    }
                }
            };

            // Act
            var localizedFileItems = ControllerLocalizationAssert.Validate(
                _projectPath,
                @"\Fakes\Controller\Validate\Resources\Controllers",
                @"\Fakes\Controller\Validate\Controllers",
                predefinedLocalizedFileItems
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