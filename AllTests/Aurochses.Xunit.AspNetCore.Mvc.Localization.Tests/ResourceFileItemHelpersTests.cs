using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class ResourceFileItemHelpersTests
    {
        private readonly string _projectPath;

        public ResourceFileItemHelpersTests()
        {
            _projectPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        [Fact]
        public void GetResourceFileItems_ThrowsException_WhenResourceFileHasInvalidName()
        {
            // Arrange
            var cultures = new HashSet<string>();

            // Act & Assert
            var exception = Assert.Throws<Exception>(
                () => ResourceFileItemHelpers.GetResourceFileItems(
                    _projectPath,
                    @"\Fakes\ResourceFileItemHelpers\GetResourceFileItems\HasInvalidName",
                    ref cultures
                )
            );
            Assert.Equal(@"Resource file '\Fakes\ResourceFileItemHelpers\GetResourceFileItems\HasInvalidName\Index.ru.RU.resx' has invalid name.", exception.Message);
        }

        [Fact]
        public void GetResourceFileItems_ThrowsException_WhenResourceFileMustBeAdded()
        {
            // Arrange
            var cultures = new HashSet<string> {"ru-RU"};

            // Act & Assert
            var exception = Assert.Throws<Exception>(
                () => ResourceFileItemHelpers.GetResourceFileItems(
                    _projectPath,
                    @"\Fakes\ResourceFileItemHelpers\GetResourceFileItems\MustBeAdded",
                    ref cultures
                )
            );
            Assert.Equal(@"Resource file '\Fakes\ResourceFileItemHelpers\GetResourceFileItems\MustBeAdded\Index.ru-RU.resx' must be added.", exception.Message);
        }

        [Fact]
        public void GetResourceFileItems_Success()
        {
            // Arrange
            var cultures = new HashSet<string> { "", "ru-RU" };
            var list = new List<ResourceFileItem>
            {
                new ResourceFileItem(_projectPath, @"\Fakes\ResourceFileItemHelpers\GetResourceFileItems\Success", "", "Index.resx")
                {
                    Values =
                    {
                        {
                            "",
                            new Resx
                            {
                                Data = new List<Resx.DataNode>
                                {
                                    new Resx.DataNode
                                    {
                                        Name = "TestName",
                                        Value = "TestValue"
                                    },
                                    new Resx.DataNode
                                    {
                                        Name = "SecondTestName",
                                        Value = "SecondTestValue"
                                    }
                                }
                            }
                        },
                        {
                            "ru-RU",
                            new Resx
                            {
                                Data = new List<Resx.DataNode>
                                {
                                    new Resx.DataNode
                                    {
                                        Name = "TestName",
                                        Value = "TestValue ru-RU"
                                    },
                                    new Resx.DataNode
                                    {
                                        Name = "SecondTestName",
                                        Value = "SecondTestValue ru-RU"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Act
            var refCultures = new HashSet<string>();
            var resourceFileItems = ResourceFileItemHelpers.GetResourceFileItems(_projectPath, @"\Fakes\ResourceFileItemHelpers\GetResourceFileItems\Success", ref refCultures);

            // Assert
            Assert.Equal(2, refCultures.Count);
            foreach (var culture in cultures)
            {
                Assert.NotNull(refCultures.FirstOrDefault(x => x == culture));
            }

            Assert.Single(resourceFileItems);
            foreach (var item in list)
            {
                var resourceFileItem = resourceFileItems.FirstOrDefault(x => x.GetFullPath() == item.GetFullPath());

                Assert.NotNull(resourceFileItem);
                Assert.Equal(2, resourceFileItem.Values.Count);

                foreach (var culture in cultures)
                {
                    foreach (var data in item.Values[culture].Data)
                    {
                        var resourceFileItemData = resourceFileItem.Values[culture].Data.FirstOrDefault(x => x.Name == data.Name);

                        Assert.NotNull(resourceFileItemData);
                        Assert.Equal(data.Value, resourceFileItemData.Value);
                    }
                }
            }
        }
    }
}