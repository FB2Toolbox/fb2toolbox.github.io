using System;
using System.ComponentModel;
using AwesomeAssertions;
using FB2Toolbox.Utilities;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class FB2EncoderFallbackBufferTests
    {
        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FB2EncoderFallbackBuffer handles unknown character")]
        public void FB2EncoderFallbackBuffer_Fallback_HandlesUnknownChar()
        {
            // Arrange
            var buffer = new FB2EncoderFallbackBuffer();
            char unknownChar = '\u2022'; // bullet point

            // Act
            bool result = buffer.Fallback(unknownChar, 0);

            // Assert
            result.Should().BeTrue("Fallback should return true for unknown character");
            buffer.Remaining.Should().BePositive("Should have characters to output");
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FB2EncoderFallbackBuffer GetNextChar returns correct entity")]
        public void FB2EncoderFallbackBuffer_GetNextChar_ReturnsEntity()
        {
            // Arrange
            var buffer = new FB2EncoderFallbackBuffer();
            char unknownChar = 'A'; // ASCII 65

            // Act
            buffer.Fallback(unknownChar, 0);
            char firstChar = buffer.GetNextChar();

            // Assert
            firstChar.Should().Be('&', "First character should be &");
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FB2EncoderFallbackBuffer handles surrogate pair")]
        public void FB2EncoderFallbackBuffer_Fallback_HandlesSurrogatePair()
        {
            // Arrange
            var buffer = new FB2EncoderFallbackBuffer();
            char high = '\uD800';
            char low = '\uDC00';

            // Act
            bool result = buffer.Fallback(high, low, 0);

            // Assert
            result.Should().BeTrue("Fallback should handle surrogate pair");
            buffer.Remaining.Should().BePositive("Should have characters to output");
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FB2EncoderFallbackBuffer throws on recursive fallback")]
        public void FB2EncoderFallbackBuffer_Fallback_ThrowsOnRecursive()
        {
            // Arrange
            var buffer = new FB2EncoderFallbackBuffer();

            // Act
            Action act = () =>
            {
                buffer.Fallback('A', 0);
                buffer.Fallback('B', 0); // Should throw
            };

            // Assert - exception expected
            act.Should().Throw<Exception>();
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FB2EncoderFallbackBuffer MovePrevious works")]
        public void FB2EncoderFallbackBuffer_MovePrevious_Works()
        {
            // Arrange
            var buffer = new FB2EncoderFallbackBuffer();
            buffer.Fallback('A', 0);
            buffer.GetNextChar();
            buffer.GetNextChar();

            // Act
            bool result = buffer.MovePrevious();

            // Assert
            result.Should().BeTrue("MovePrevious should succeed");
        }

        [Fact]
        [Trait("Category", "FileUtils")]
        [Description("Test FB2EncoderFallbackBuffer Remaining property")]
        public void FB2EncoderFallbackBuffer_Remaining_ReturnsCorrectCount()
        {
            // Arrange
            var buffer = new FB2EncoderFallbackBuffer();
            buffer.Fallback('A', 0);
            int initialRemaining = buffer.Remaining;

            // Act
            buffer.GetNextChar();
            int afterOneChar = buffer.Remaining;

            // Assert
            afterOneChar.Should().BeLessThan(initialRemaining, "Remaining should decrease");
        }
    }
}
