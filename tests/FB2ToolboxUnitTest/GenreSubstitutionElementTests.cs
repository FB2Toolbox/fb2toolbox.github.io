using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class GenreSubstitutionElementTests
    {
        [Fact]
        [Trait("Category", "Config")]
        [Description("Test GenreSubstitutionElement properties")]
        public void GenreSubstitutionElement_Properties_Work()
        {
            // Arrange
            var element = new GenreSubstitutionElement();

            // Act
            element.From = "sf";
            element.To = "Science Fiction";

            // Assert
            element.From.Should().Be("sf");
            element.To.Should().Be("Science Fiction");
        }

        [Fact]
        [Trait("Category", "Config")]
        [Description("Test GenreSubstitutionElement ToString")]
        public void GenreSubstitutionElement_ToString_ReturnsFormattedString()
        {
            // Arrange
            var element = new GenreSubstitutionElement
            {
                From = "sf",
                To = "Science Fiction"
            };

            // Act
            string result = element.ToString();

            // Assert
            result.Should().Be("Science Fiction (sf)");
        }

        [Fact]
        [Trait("Category", "Config")]
        [Description("Test GenreSubstitutionElement CompareTo")]
        public void GenreSubstitutionElement_CompareTo_Works()
        {
            // Arrange
            var element1 = new GenreSubstitutionElement
            {
                From = "sf",
                To = "Science Fiction"
            };
            var element2 = new GenreSubstitutionElement
            {
                From = "fantasy",
                To = "Fantasy"
            };

            // Act
            int result = element1.CompareTo(element2);

            // Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        [Trait("Category", "Config")]
        [Description("Test GenreSubstitutionElement CompareTo with non-GenreSubstitutionElement")]
        public void GenreSubstitutionElement_CompareTo_WithOtherType_ReturnsZero()
        {
            // Arrange
            var element = new GenreSubstitutionElement
            {
                From = "sf",
                To = "Science Fiction"
            };
            object other = "string";

            // Act
            int result = element.CompareTo(other);

            // Assert
            result.Should().Be(0);
        }
    }
}
