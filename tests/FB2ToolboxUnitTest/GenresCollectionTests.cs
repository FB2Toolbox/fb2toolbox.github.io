using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class GenresCollectionTests
    {
        [Fact]
        [Trait("Category", "Config")]
        [Description("Test GenresCollection FindSubstitution returns original if not found")]
        public void GenresCollection_FindSubstitution_ReturnsOriginalWhenNotFound()
        {
            // Arrange
            var collection = new GenresCollection();

            // Act
            string result = collection.FindSubstitution("unknown_genre");

            // Assert
            result.Should().Be("unknown_genre");
        }

        [Fact]
        [Trait("Category", "Config")]
        [Description("Test GenresCollection FindSubstitution returns empty for null")]
        public void GenresCollection_FindSubstitution_ReturnsEmptyForNull()
        {
            // Arrange
            var collection = new GenresCollection();

            // Act
            string result = collection.FindSubstitution(null);

            // Assert
            result.Should().Be(string.Empty);
        }

        [Fact]
        [Trait("Category", "Config")]
        [Description("Test GenresCollection FindSubstitution returns empty for empty string")]
        public void GenresCollection_FindSubstitution_ReturnsEmptyForEmptyString()
        {
            // Arrange
            var collection = new GenresCollection();

            // Act
            string result = collection.FindSubstitution(string.Empty);

            // Assert
            result.Should().Be(string.Empty);
        }
    }
}
