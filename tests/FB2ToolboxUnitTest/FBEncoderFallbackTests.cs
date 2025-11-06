using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox.Utilities;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class FBEncoderFallbackTests
    {
        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FBEncoderFallback CreateFallbackBuffer")]
        public void FBEncoderFallback_CreateFallbackBuffer_ReturnsBuffer()
        {
            // Arrange
            var fallback = new FBEncoderFallback();

            // Act
            var buffer = fallback.CreateFallbackBuffer();

            // Assert
            buffer.Should().NotBeNull("Should return a buffer");
            buffer.Should().BeOfType<FB2EncoderFallbackBuffer>();
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FBEncoderFallback MaxCharCount")]
        public void FBEncoderFallback_MaxCharCount_Returns8()
        {
            // Arrange
            var fallback = new FBEncoderFallback();

            // Act
            int maxCount = fallback.MaxCharCount;

            // Assert
            maxCount.Should().Be(8, "MaxCharCount should be 8");
        }
    }
}
