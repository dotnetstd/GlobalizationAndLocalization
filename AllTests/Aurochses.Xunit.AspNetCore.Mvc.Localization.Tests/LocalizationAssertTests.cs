using System;
using System.Collections.Generic;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class LocalizationAssertTests
    {
        private readonly string _projectPath;

        public LocalizationAssertTests()
        {
            _projectPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        [Fact]
        public void Validate_ThrowsException_WhenResourceFileNotFound()
        {
            // Arrange
            var localizedFileItems = new List<LocalizedFileItem>
            {
                new LocalizedFileItem(_projectPath, @"\Fakes\LocalizationAssert\Validate\NotFound\Views", "", "Index.cshtml")
                {
                    Names =
                    {
                        "TestName",
                        "SecondTestName"
                    }
                }
            };

            // Act & Assert
            var exception = Assert.Throws<Exception>(
                () => LocalizationAssert.Validate(
                    _projectPath,
                    @"\Fakes\LocalizationAssert\Validate\NotFound\Resources\Views",
                    localizedFileItems
                )
            );
            Assert.Equal(@"Resource file for '\Fakes\LocalizationAssert\Validate\NotFound\Views\Index.cshtml' not found.", exception.Message);
        }

        [Fact]
        public void Validate_ThrowsException_WhenResourceFileHasNoValueForNameFromLocalizedFile()
        {
            // Arrange
            var localizedFileItems = new List<LocalizedFileItem>
            {
                new LocalizedFileItem(_projectPath, @"\Fakes\LocalizationAssert\Validate\HasNoValue\Views", "", "Index.cshtml")
                {
                    Names =
                    {
                        "TestName",
                        "SecondTestName"
                    }
                }
            };

            // Act & Assert
            var exception = Assert.Throws<Exception>(
                () => LocalizationAssert.Validate(
                    _projectPath,
                    @"\Fakes\LocalizationAssert\Validate\HasNoValue\Resources\Views",
                    localizedFileItems
                )
            );
            Assert.Equal(@"Resource file '\Fakes\LocalizationAssert\Validate\HasNoValue\Resources\Views\Index.resx' has no value for 'SecondTestName' from localized file '\Fakes\LocalizationAssert\Validate\HasNoValue\Views\Index.cshtml'.", exception.Message);
        }

        [Fact]
        public void Validate_ThrowsException_WhenValueFromResourceFileIsNotUsed()
        {
            // Arrange
            var localizedFileItems = new List<LocalizedFileItem>
            {
                new LocalizedFileItem(_projectPath, @"\Fakes\LocalizationAssert\Validate\ValueIsNotUsed\Views", "", "Index.cshtml")
                {
                    Names =
                    {
                        "TestName",
                        "SecondTestName"
                    }
                }
            };

            // Act & Assert
            var exception = Assert.Throws<Exception>(
                () => LocalizationAssert.Validate(
                    _projectPath,
                    @"\Fakes\LocalizationAssert\Validate\ValueIsNotUsed\Resources\Views",
                    localizedFileItems
                )
            );
            Assert.Equal(@"Value for 'NotUsedTestName' from resource file '\Fakes\LocalizationAssert\Validate\ValueIsNotUsed\Resources\Views\Index.resx' is not used.", exception.Message);
        }

        [Fact]
        public void Validate_ThrowsException_WhenResourceFileNotUsed()
        {
            // Arrange
            var localizedFileItems = new List<LocalizedFileItem>
            {
                new LocalizedFileItem(_projectPath, @"\Fakes\LocalizationAssert\Validate\NotUsed\Views", "", "Index.cshtml")
                {
                    Names =
                    {
                        "TestName",
                        "SecondTestName"
                    }
                }
            };

            // Act & Assert
            var exception = Assert.Throws<Exception>(
                () => LocalizationAssert.Validate(
                    _projectPath,
                    @"\Fakes\LocalizationAssert\Validate\NotUsed\Resources\Views",
                    localizedFileItems
                )
            );
            Assert.Equal(@"Resource file '\Fakes\LocalizationAssert\Validate\NotUsed\Resources\Views\NotUsed\Index.resx' not used.", exception.Message);
        }

        [Fact]
        public void Validate_Success()
        {
            // Arrange
            var localizedFileItems = new List<LocalizedFileItem>
            {
                new LocalizedFileItem(_projectPath, @"\Fakes\LocalizationAssert\Validate\Success\Views", "", "Index.cshtml")
                {
                    Names =
                    {
                        "TestName",
                        "SecondTestName"
                    }
                }
            };

            // Act & Assert
            LocalizationAssert.Validate(
                _projectPath,
                @"\Fakes\LocalizationAssert\Validate\Success\Resources\Views",
                localizedFileItems
            );
        }
    }
}