using System;
using FB2Toolbox.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FB2ToolboxUnitTest
{
    [TestClass]
    public class FileUtilsTests
    {
        [TestClass]
        public class FB2EncoderFallbackBufferTests
        {
            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FB2EncoderFallbackBuffer handles unknown character")]
            public void FB2EncoderFallbackBuffer_Fallback_HandlesUnknownChar()
            {
                // Arrange
                var buffer = new FB2EncoderFallbackBuffer();
                char unknownChar = '\u2022'; // bullet point

                // Act
                bool result = buffer.Fallback(unknownChar, 0);

                // Assert
                Assert.IsTrue(result, "Fallback should return true for unknown character");
                Assert.IsTrue(buffer.Remaining > 0, "Should have characters to output");
            }

            [TestMethod]
            [TestCategory("FileUtils")]
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
                Assert.AreEqual('&', firstChar, "First character should be &");
            }

            [TestMethod]
            [TestCategory("FileUtils")]
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
                Assert.IsTrue(result, "Fallback should handle surrogate pair");
                Assert.IsTrue(buffer.Remaining > 0, "Should have characters to output");
            }

            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FB2EncoderFallbackBuffer throws on recursive fallback")]
            [ExpectedException(typeof(Exception))]
            public void FB2EncoderFallbackBuffer_Fallback_ThrowsOnRecursive()
            {
                // Arrange
                var buffer = new FB2EncoderFallbackBuffer();

                // Act
                buffer.Fallback('A', 0);
                buffer.Fallback('B', 0); // Should throw

                // Assert - exception expected
            }

            [TestMethod]
            [TestCategory("FileUtils")]
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
                Assert.IsTrue(result, "MovePrevious should succeed");
            }

            [TestMethod]
            [TestCategory("FileUtils")]
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
                Assert.IsTrue(initialRemaining > afterOneChar, "Remaining should decrease");
            }
        }

        [TestClass]
        public class FBEncoderFallbackTests
        {
            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FBEncoderFallback CreateFallbackBuffer")]
            public void FBEncoderFallback_CreateFallbackBuffer_ReturnsBuffer()
            {
                // Arrange
                var fallback = new FBEncoderFallback();

                // Act
                var buffer = fallback.CreateFallbackBuffer();

                // Assert
                Assert.IsNotNull(buffer, "Should return a buffer");
                Assert.IsInstanceOfType(buffer, typeof(FB2EncoderFallbackBuffer));
            }

            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FBEncoderFallback MaxCharCount")]
            public void FBEncoderFallback_MaxCharCount_Returns8()
            {
                // Arrange
                var fallback = new FBEncoderFallback();

                // Act
                int maxCount = fallback.MaxCharCount;

                // Assert
                Assert.AreEqual(8, maxCount, "MaxCharCount should be 8");
            }
        }

        [TestClass]
        public class FileMetadataTests
        {
            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FileMetadata initialization")]
            public void FileMetadata_Constructor_Initializes()
            {
                // Arrange & Act
                var metadata = new FileMetadata();

                // Assert
                Assert.IsNotNull(metadata, "Metadata should be created");
                Assert.AreEqual(string.Empty, metadata.Description);
            }

            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FileMetadata AddMetadata and GetMetadata")]
            public void FileMetadata_AddAndGetMetadata_Works()
            {
                // Arrange
                var metadata = new FileMetadata();

                // Act
                metadata.AddMetadata(DescriptionElements.Title, "Test Title");
                string result = metadata.GetMetadata(DescriptionElements.Title);

                // Assert
                Assert.AreEqual("Test Title", result);
            }

            [TestMethod]
            [TestCategory("FileUtils")]
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
                Assert.AreEqual("Ivan", result);
            }

            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FileMetadata SubstitutePart with required attribute")]
            public void FileMetadata_SubstitutePart_ReplacesPlaceholders()
            {
                // Arrange
                var metadata = new FileMetadata();
                metadata.AddMetadata(DescriptionElements.Title, "Test Book");

                // Act
                string result = metadata.SubstitutePart("Book: (Title)");

                // Assert
                Assert.AreEqual("Book: Test Book", result);
            }

            [TestMethod]
            [TestCategory("FileUtils")]
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
                Assert.AreEqual("Book: Test Book", result);
            }

            [TestMethod]
            [TestCategory("FileUtils")]
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
                Assert.AreEqual("Book: Test Book Series One", result);
            }

            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FileMetadata SubstitutePart returns empty for missing required")]
            public void FileMetadata_SubstitutePart_ReturnsEmptyForMissingRequired()
            {
                // Arrange
                var metadata = new FileMetadata();
                metadata.AddMetadata(DescriptionElements.Title, string.Empty);

                // Act
                string result = metadata.SubstitutePart("Book: (Title)");

                // Assert
                Assert.AreEqual(string.Empty, result);
            }
        }

        [TestClass]
        public class FilePropertiesTests
        {
            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FileProperties default values")]
            public void FileProperties_DefaultValues_AreCorrect()
            {
                // Arrange & Act
                var props = new FileProperties();

                // Assert
                Assert.IsFalse(props.AuthorFirstNameChange);
                Assert.IsFalse(props.AuthorLastNameChange);
                Assert.IsFalse(props.AuthorMiddleNameChange);
                Assert.IsFalse(props.GengeChange);
                Assert.IsFalse(props.SeriesChange);
                Assert.IsFalse(props.NumberChange);
                Assert.IsFalse(props.TitleChange);
            }

            [TestMethod]
            [TestCategory("FileUtils")]
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
                Assert.IsTrue(props.AuthorFirstNameChange);
                Assert.AreEqual("Ivan", props.AuthorFirstName);
                Assert.IsTrue(props.TitleChange);
                Assert.AreEqual("New Title", props.Title);
            }
        }

        [TestClass]
        public class FileOperationResultTests
        {
            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FileOperationResult default values")]
            public void FileOperationResult_DefaultValues_AreNull()
            {
                // Arrange & Act
                var result = new FileOperationResult();

                // Assert
                Assert.IsNull(result.NewFullName);
                Assert.IsNull(result.NewFileName);
                Assert.IsFalse(result.Skipped);
            }

            [TestMethod]
            [TestCategory("FileUtils")]
            [Description("Test FileOperationResult can set properties")]
            public void FileOperationResult_SetProperties_Works()
            {
                // Arrange
                var result = new FileOperationResult();

                // Act
                result.NewFullName = @"C:\test\file.fb2";
                result.NewFileName = "file.fb2";
                result.Skipped = true;

                // Assert
                Assert.AreEqual(@"C:\test\file.fb2", result.NewFullName);
                Assert.AreEqual("file.fb2", result.NewFileName);
                Assert.IsTrue(result.Skipped);
            }
        }
    }
}
