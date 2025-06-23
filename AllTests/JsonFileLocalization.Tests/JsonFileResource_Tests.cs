using System;
using System.Globalization;
using System.Linq;
using FluentAssertions;
using JsonFileLocalization.Resource;
using JsonFileLocalization.Tests.TestData;
using JsonFileLocalization.Tests.TestData.Models;
using Xunit;

namespace JsonFileLocalization.Tests
{
    public class JsonFileResource_Tests
    {
        [Fact]
        public void GetValue_WhenCalledWithCorrectPath_ReturnsCorrectValue()
        {
            //Arrange
            var manager = TestJsonFileResourceManager.GetResourceManager(CultureSuffixStrategy.TwoLetterISO6391AndCountryCode);

            //Act
            var resource = manager.GetResource("_Layout", String.Empty, new CultureInfo("ru-RU"));
            var testString = resource.GetValue<string>("TestString").Value;
            var testArray = resource.GetValue<string[]>("Inner.TestArray").Value;
            var testObject = resource.GetValue<TestObject>("TestObject").Value;

            //Assert
            testString.Should().Be("Test");
            testArray.Should().Contain(new[] { "One", "Two" });
            testObject.Value.Should().Be("Something");
            testObject.Id.Should().Be(1);
        }

        [Fact]
        public void GetValue_WhenCalledWithIncorrectType_ReturnsDefaultValueAndWritesInLog()
        {
            //Arrange
            var loggerFactory = TestJsonFileResourceManager.GetLoggerFactory();
            var settings = TestJsonFileResourceManager.GetSettings(CultureSuffixStrategy.TwoLetterISO6391AndCountryCode);
            var manager = new JsonFileResourceManager(settings, loggerFactory.Factory);

            //Act
            var resource = manager.GetResource("_Layout", String.Empty, new CultureInfo("ru-RU"));
            var value = resource.GetValue<int>("TestObject").Value;

            //Assert
            value.Should().Be(default(int));
            var loggerCalls = loggerFactory.LoggerCalls();
            loggerCalls.Count().Should().Be(1);
        }

        [Fact]
        public void GetValue_WhenFileIsInSubfolder_ReturnsResource()
        {
            //Arrange
            var manager = TestJsonFileResourceManager.GetResourceManager(CultureSuffixStrategy.TwoLetterISO6391AndCountryCode);

            //Act
            var resource = manager.GetResource("_Layout", String.Empty, new CultureInfo("fr-CA"));
            var resource2 = manager.GetResource("Some.Folder.Name", String.Empty, new CultureInfo("en-US"));
            var resource3 = manager.GetResource("Some.Data.File", String.Empty, new CultureInfo("en-US"));

            resource.GetValue<string>("Data").Value.Should().Be("Fr value");
            resource2.GetValue<string>("Data").Value.Should().Be("Something");
            resource3.GetValue<string>("Data").Value.Should().Be("Data value");
        }
    }
}
