using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox.Utilities;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class FilePropertiesTests
    {
        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileProperties default values")]
        public void FileProperties_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var props = new FileProperties();

            // Assert
            props.AuthorFirstNameChange.Should().BeFalse();
            props.AuthorLastNameChange.Should().BeFalse();
            props.AuthorMiddleNameChange.Should().BeFalse();
            props.GengeChange.Should().BeFalse();
            props.SeriesChange.Should().BeFalse();
            props.NumberChange.Should().BeFalse();
            props.TitleChange.Should().BeFalse();
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FileProperties can set properties")]
        public void FileProperties_SetProperties_Works()
        {
            // Arrange
            var props = new FileProperties();

            // Act
            props.AuthorFirstNameChange = true;
            props.AuthorFirstName = "Ivan";
            props.TitleChange = true;
            props.Title = "New Title";

            // Assert
            props.AuthorFirstNameChange.Should().BeTrue();
            props.AuthorFirstName.Should().Be("Ivan");
            props.TitleChange.Should().BeTrue();
            props.Title.Should().Be("New Title");
        }
    }
}
