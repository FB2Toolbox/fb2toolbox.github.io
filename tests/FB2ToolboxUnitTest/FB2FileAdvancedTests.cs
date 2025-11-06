using System;
using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox.Utilities;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class FB2FileAdvancedTests
    {
        [Fact]
        [Category("FB2 File")]
        [Description("Test FB2File CompareTo returns 0 for same files")]
        public void FB2File_CompareTo_ReturnsSameForIdenticalFiles()
        {
            // Arrange
            var file1 = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");
            var file2 = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act
            int result = file1.CompareTo(file2);

            // Assert
            result.Should().Be(0, "Same files should compare as equal");
        }

        [Fact]
        [Category("FB2 File")]
        [Description("Test FB2File CompareTo throws InvalidCastException for invalid type")]
        public void FB2File_CompareTo_ThrowsExceptionForInvalidType()
        {
            // Arrange
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");
            var invalidObject = "not a file";

            // Act
            Action act = () => file.CompareTo(invalidObject);

            // Assert - exception expected
            act.Should().Throw<InvalidCastException>("Comparing to non-FB2File should throw InvalidCastException");
        }

        [Fact]
        [Category("FB2 File")]
        [Description("Test FB2File ToString returns file name")]
        public void FB2File_ToString_ReturnsFileName()
        {
            // Arrange
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act
            string result = file.ToString();

            // Assert
            result.Should().Be("Макиавелли Николо - Государь.fb2");
        }

        [Fact]
        [Category("FB2 File")]
        [Description("Test FB2File IsZIP returns false for .fb2 files")]
        public void FB2File_IsZIP_ReturnsFalseForFB2()
        {
            // Arrange
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act
            bool result = file.IsZIP();

            // Assert
            result.Should().BeFalse("FB2 file should not be identified as ZIP");
        }

        [Fact]
        [Category("FB2 File")]
        [Description("Test FB2File IsZIP returns true for .fb2.zip files")]
        public void FB2File_IsZIP_ReturnsTrueForFB2ZIP()
        {
            // Arrange
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2.zip");

            // Act
            bool result = file.IsZIP();

            // Assert
            result.Should().BeTrue("FB2.ZIP file should be identified as ZIP");
        }

        [Fact]
        [Category("FB2 File")]
        [Description("Test FB2File IsZIP with filename parameter returns false for .fb2 files")]
        public void FB2File_IsZIP_WithFilename_ReturnsFalseForFB2()
        {
            // Arrange
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");
            string testPath = @"TestFiles\Макиавелли Николо - Государь.fb2";

            // Act
            bool result = file.IsZIP(testPath);

            // Assert
            result.Should().BeFalse("FB2 file should not be identified as ZIP");
        }

        [Fact]
        [Category("FB2 File")]
        [Description("Test FB2File FileInformation property provides correct file details")]
        public void FB2File_FileInformation_ProvidesCorrectDetails()
        {
            // Arrange
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act
            var fileInfo = file.FileInformation;

            // Assert
            fileInfo.Should().NotBeNull($"{nameof(file.FileInformation)} should not be null");
            fileInfo.Exists.Should().BeTrue("File should exist");
            fileInfo.Name.Should().Be("Макиавелли Николо - Государь.fb2", "File name should match");
            fileInfo.Length.Should().BePositive("File should have content");
        }

        [Fact]
        [Category("FB2 File")]
        [Description("Test FB2File Metadata property contains description")]
        public void FB2File_Metadata_ContainsDescription()
        {
            // Arrange
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act
            var metadata = file.Metadata;

            // Assert
            metadata.Should().NotBeNull("Metadata should not be null");
            metadata.Description.Should().NotBeNull("Description should not be null");
            metadata.Description.Length.Should().BePositive("Description should have content");
        }

        [Fact]
        [Category("FB2 File")]
        [Description("Test FB2File UpdateFileInfo changes FileInformation")]
        public void FB2File_UpdateFileInfo_ChangesFileInformation()
        {
            // Arrange
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");
            string originalName = file.FileInformation.Name;

            // Act
            file.UpdateFileInfo(@"TestFiles\Макиавелли Николо - Государь.fb2.zip");
            string newName = file.FileInformation.Name;

            // Assert
            newName.Should().NotBe(originalName, "File name should be different after update");
            newName.Should().Be("Макиавелли Николо - Государь.fb2.zip", "New file name should match");
        }

        [Fact]
        [Category("FB2 File")]
        [Description("Test FB2File BookInternalEncoding is set via reflection")]
        public void FB2File_BookInternalEncoding_IsSet()
        {
            // Arrange
            var file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act - access via reflection since it's protected internal
            var type = file.GetType();
            var property = type.GetProperty("BookInternalEncoding",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public);
            var value = property.GetValue(file, null) as string;

            // Assert
            value.Should().NotBeNull("BookInternalEncoding should be set");
            value.Length.Should().BePositive("BookInternalEncoding should not be empty");
        }
    }
}
