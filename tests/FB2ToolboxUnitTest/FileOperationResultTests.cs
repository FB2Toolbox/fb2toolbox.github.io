using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox.Utilities;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class FileOperationResultTests
    {
        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileOperationResult default values")]
        public void FileOperationResult_DefaultValues_AreNull()
        {
            // Arrange & Act
            var result = new FileOperationResult();

            // Assert
            result.NewFullName.Should().BeNull();
            result.NewFileName.Should().BeNull();
            result.Skipped.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileOperationResult can set properties")]
        public void FileOperationResult_SetProperties_Works()
        {
            // Arrange
            var result = new FileOperationResult();

            // Act
            result.NewFullName = @"C:\test\file.fb2";
            result.NewFileName = "file.fb2";
            result.Skipped = true;

            // Assert
            result.NewFullName.Should().Be(@"C:\test\file.fb2");
            result.NewFileName.Should().Be("file.fb2");
            result.Skipped.Should().BeTrue();
        }
    }
}
