using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class CommandElementTests
    {
        [Fact]
        [Trait("Category", "Config")]
        [Description("Test CommandElement properties")]
        public void CommandElement_Properties_Work()
        {
            // Arrange
            var element = new CommandElement();

            // Act
            element.Name = "Open with Notepad";
            element.FileName = "notepad.exe";
            element.Arguments = "{0}";
            element.CreateNoWindow = false;
            element.OnlyWithExtension = ".fb2";
            element.WaitAndReload = true;

            // Assert
            element.Name.Should().Be("Open with Notepad");
            element.FileName.Should().Be("notepad.exe");
            element.Arguments.Should().Be("{0}");
            element.CreateNoWindow.Should().BeFalse();
            element.OnlyWithExtension.Should().Be(".fb2");
            element.WaitAndReload.Should().BeTrue();
        }
    }
}
