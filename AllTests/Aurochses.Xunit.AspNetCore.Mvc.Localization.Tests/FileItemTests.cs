using System.IO;
using Xunit;

namespace Aurochses.Xunit.AspNetCore.Mvc.Localization.Tests
{
    public class FileItemTests
    {
        private const string ProjectPath = "testProjectPath";
        private const string DirectoryPath = "testDirectoryPath";
        private const string RelativePath = "testRelativePath";
        private const string FileName = "testFileName.cs";

        private readonly FileItem _fileItem;

        public FileItemTests()
        {
            _fileItem = new FileItem(ProjectPath, DirectoryPath, RelativePath, FileName);
        }

        [Fact]
        public void ProjectPath_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(ProjectPath, _fileItem.ProjectPath);
        }

        [Fact]
        public void DirectoryPath_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(DirectoryPath, _fileItem.DirectoryPath);
        }

        [Fact]
        public void RelativePath_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(RelativePath, _fileItem.RelativePath);
        }

        [Fact]
        public void FileName_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(FileName, _fileItem.FileName);
        }

        [Fact]
        public void FileNameWithoutExtension_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(Path.GetFileNameWithoutExtension(FileName), _fileItem.FileNameWithoutExtension);
        }

        [Fact]
        public void GetFullPath_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal($@"{ProjectPath}{DirectoryPath}{RelativePath}\{FileName}", _fileItem.GetFullPath());
        }

        [Fact]
        public void GetFullRelativePath_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal($@"{DirectoryPath}{RelativePath}\{FileName}", _fileItem.GetFullRelativePath());
        }
    }
}