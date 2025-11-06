using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class RenameProfileElementTests
    {
        [Fact]
        [Trait("Category", "Config")]
        [Description("Test RenameProfileElement properties")]
        public void RenameProfileElement_Properties_Work()
        {
            // Arrange
            var element = new RenameProfileElement();

            // Act
            element.Name = "Test Profile";
            element.Path = @"(AuthorLastName)\(SequenceName)";
            element.FileName = "(AuthorLastName) - (Title)";

            // Assert
            element.Name.Should().Be("Test Profile");
            element.Path.Should().Be(@"(AuthorLastName)\(SequenceName)");
            element.FileName.Should().Be("(AuthorLastName) - (Title)");
        }

        [Fact]
        [Trait("Category", "Config")]
        [Description("Test RenameProfileElement CharacterSubstitution collection")]
        public void RenameProfileElement_CharacterSubstitution_IsNotNull()
        {
            // Arrange
            var element = new RenameProfileElement();

            // Act
            var substitutions = element.CharacterSubstitution;

            // Assert
            substitutions.Should().NotBeNull();
        }
    }
}
