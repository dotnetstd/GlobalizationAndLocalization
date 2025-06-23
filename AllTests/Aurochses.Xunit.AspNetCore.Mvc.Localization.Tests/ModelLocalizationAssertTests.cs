using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class ModelLocalizationAssertTests
    {
        private readonly string _projectPath;

        public ModelLocalizationAssertTests()
        {
            _projectPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        [Fact]
        public void Validate_Success()
        {
            // Arrange
            var list = new List<LocalizedFileItem>
            {
                new LocalizedFileItem(_projectPath, @"\Fakes\ModelLocalizationAssert\Validate\Models", "", "HomeViewModel.cs")
                {
                    Names =
                    {
                        "Email",
                        "Email.Prompt",
                        "Email.Description"
                    }
                }
            };

            // Act
            var localizedFileItems = ModelLocalizationAssert.Validate(
                typeof(ModelLocalizationAssertTests).GetTypeInfo().Assembly,
                _projectPath,
                @"\Fakes\ModelLocalizationAssert\Validate\Resources\Models",
                @"\Fakes\ModelLocalizationAssert\Validate\Models"
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