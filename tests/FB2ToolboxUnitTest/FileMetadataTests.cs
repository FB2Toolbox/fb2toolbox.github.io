using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox.Utilities;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class FileMetadataTests
    {
        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileMetadata initialization")]
        public void FileMetadata_Constructor_Initializes()
        {
            // Arrange & Act
            var metadata = new FileMetadata();

            // Assert
            metadata.Should().NotBeNull("Metadata should be created");
            metadata.Description.Should().Be(string.Empty);
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileMetadata AddMetadata and GetMetadata")]
        public void FileMetadata_AddAndGetMetadata_Works()
        {
            // Arrange
            var metadata = new FileMetadata();

            // Act
            metadata.AddMetadata(DescriptionElements.Title, "Test Title");
            string result = metadata.GetMetadata(DescriptionElements.Title);

            // Assert
            result.Should().Be("Test Title");
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileMetadata AddMetadata with index")]
        public void FileMetadata_AddMetadataWithIndex_Works()
        {
            // Arrange
            var metadata = new FileMetadata();

            // Act
            metadata.AddMetadata(DescriptionElements.AuthorFirstName, 0, "Ivan");
            metadata.AddMetadata(DescriptionElements.AuthorFirstName, 1, "Petr");
            string result = metadata.GetMetadata(DescriptionElements.AuthorFirstName);

            // Assert
            result.Should().Be("Ivan");
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileMetadata SubstitutePart with required attribute")]
        public void FileMetadata_SubstitutePart_ReplacesPlaceholders()
        {
            // Arrange
            var metadata = new FileMetadata();
            metadata.AddMetadata(DescriptionElements.Title, "Test Book");

            // Act
            string result = metadata.SubstitutePart("Book: (Title)");

            // Assert
            result.Should().Be("Book: Test Book");
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileMetadata SubstitutePart with optional attribute")]
        public void FileMetadata_SubstitutePart_RemovesOptionalBrackets()
        {
            // Arrange
            var metadata = new FileMetadata();
            metadata.AddMetadata(DescriptionElements.Title, "Test Book");
            metadata.AddMetadata(DescriptionElements.SequenceName, string.Empty);

            // Act
            string result = metadata.SubstitutePart("Book: (Title)[SequenceName]");

            // Assert
            result.Should().Be("Book: Test Book");
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileMetadata SubstitutePart with optional attribute that has value")]
        public void FileMetadata_SubstitutePart_IncludesOptionalWithValue()
        {
            // Arrange
            var metadata = new FileMetadata();
            metadata.AddMetadata(DescriptionElements.Title, "Test Book");
            metadata.AddMetadata(DescriptionElements.SequenceName, "Series One");

            // Act
            string result = metadata.SubstitutePart("Book: (Title) [SequenceName]");

            // Assert
            result.Should().Be("Book: Test Book Series One");
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileMetadata SubstitutePart returns empty for missing required")]
        public void FileMetadata_SubstitutePart_ReturnsEmptyForMissingRequired()
        {
            // Arrange
            var metadata = new FileMetadata();
            metadata.AddMetadata(DescriptionElements.Title, string.Empty);

            // Act
            string result = metadata.SubstitutePart("Book: (Title)");

            // Assert
            result.Should().Be(string.Empty);
        }
    }
}
