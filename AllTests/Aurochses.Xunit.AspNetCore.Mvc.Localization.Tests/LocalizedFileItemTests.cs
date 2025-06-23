using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class LocalizedFileItemTests
    {
        private const string ProjectPath = "testProjectPath";
        private const string DirectoryPath = "testDirectoryPath";
        private const string RelativePath = "testRelativePath";
        private const string FileName = "testFileName.cs";

        private readonly LocalizedFileItem _localizedFileItem;

        public LocalizedFileItemTests()
        {
            _localizedFileItem = new LocalizedFileItem(ProjectPath, DirectoryPath, RelativePath, FileName);
        }

        [Fact]
        public void Inherit_FileItem()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<FileItem>(_localizedFileItem);
        }

        [Fact]
        public void Names_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_localizedFileItem.Names);
        }
    }
}