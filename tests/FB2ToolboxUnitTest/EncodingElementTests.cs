using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class EncodingElementTests
    {
        [Fact]
        [Trait("Category", "Config")]
        [Description("Test EncodingElement Name property")]
        public void EncodingElement_Name_Works()
        {
            // Arrange
            var element = new EncodingElement();

            // Act
            element.Name = "UTF-8";

            // Assert
            element.Name.Should().Be("UTF-8");
        }
    }
}
