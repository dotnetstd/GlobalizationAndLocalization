using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class ResourceFileItemTests
    {
        private const string ProjectPath = "testProjectPath";
        private const string DirectoryPath = "testDirectoryPath";
        private const string RelativePath = "testRelativePath";
        private const string FileName = "testFileName.cs";

        private readonly ResourceFileItem _resourceFileItem;

        public ResourceFileItemTests()
        {
            _resourceFileItem = new ResourceFileItem(ProjectPath, DirectoryPath, RelativePath, FileName);
        }

        [Fact]
        public void Inherit_FileItem()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<FileItem>(_resourceFileItem);
        }

        [Fact]
        public void Values_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_resourceFileItem.Values);
        }

        [Fact]
        public void GetFullRelativePath_ReturnFullRelativePath_WhenCultureIsEmpty()
        {
            // Arrange & Act & Assert
            Assert.Equal(_resourceFileItem.GetFullRelativePath(), _resourceFileItem.GetFullRelativePath(""));
        }

        [Fact]
        public void GetFullRelativePath_ReturnFullRelativePathWithCulture_WhenCultureIsNotEmpty()
        {
            // Arrange
            const string culture = "ru-RU";

            // Act
            var fullRelativePath = _resourceFileItem.GetFullRelativePath(culture);

            // Assert
            Assert.Equal($@"{DirectoryPath}{RelativePath}\{_resourceFileItem.FileNameWithoutExtension}.{culture}.resx", fullRelativePath);
        }
    }
}