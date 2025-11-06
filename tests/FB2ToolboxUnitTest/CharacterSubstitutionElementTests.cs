using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class CharacterSubstitutionElementTests
    {
        [Fact]
        [Trait("Category", "Config")]
        [Description("Test CharacterSubstitutionElement properties")]
        public void CharacterSubstitutionElement_Properties_Work()
        {
            // Arrange
            var element = new CharacterSubstitutionElement();

            // Act
            element.From = "?";
            element.To = "_";
            element.Repeat = 2;

            // Assert
            element.From.Should().Be("?");
            element.To.Should().Be("_");
            element.Repeat.Should().Be(2);
        }
    }
}
