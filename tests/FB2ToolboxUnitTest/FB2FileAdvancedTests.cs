using System;
using FB2Toolbox.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FB2ToolboxUnitTest
{
    [TestClass]
    public class FB2FileAdvancedTests
    {
        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Test FB2File CompareTo returns 0 for same files")]
        public void FB2File_CompareTo_ReturnsSameForIdenticalFiles()
        {
            // Arrange
            FB2File file1 = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");
            FB2File file2 = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act
            int result = file1.CompareTo(file2);

            // Assert
            Assert.AreEqual(0, result, "Same files should compare as equal");
        }

        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Test FB2File CompareTo throws InvalidCastException for invalid type")]
        [ExpectedException(typeof(InvalidCastException))]
        public void FB2File_CompareTo_ThrowsExceptionForInvalidType()
        {
            // Arrange
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");
            var invalidObject = "not a file";

            // Act
            file.CompareTo(invalidObject);

            // Assert - exception expected
        }

        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Test FB2File ToString returns file name")]
        public void FB2File_ToString_ReturnsFileName()
        {
            // Arrange
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act
            string result = file.ToString();

            // Assert
            Assert.AreEqual("Макиавелли Николо - Государь.fb2", result);
        }

        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Test FB2File IsZIP returns false for .fb2 files")]
        public void FB2File_IsZIP_ReturnsFalseForFB2()
        {
            // Arrange
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act
            bool result = file.IsZIP();

            // Assert
            Assert.IsFalse(result, "FB2 file should not be identified as ZIP");
        }

        [TestMethod]
        [TestCategory("FB2.ZIP File")]
        [Description("Test FB2File IsZIP returns true for .fb2.zip files")]
        public void FB2File_IsZIP_ReturnsTrueForFB2ZIP()
        {
            // Arrange
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2.zip");

            // Act
            bool result = file.IsZIP();

            // Assert
            Assert.IsTrue(result, "FB2.ZIP file should be identified as ZIP");
        }

        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Test FB2File IsZIP with filename parameter returns false for .fb2 files")]
        public void FB2File_IsZIP_WithFilename_ReturnsFalseForFB2()
        {
            // Arrange
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");
            string testPath = @"TestFiles\Макиавелли Николо - Государь.fb2";

            // Act
            bool result = file.IsZIP(testPath);

            // Assert
            Assert.IsFalse(result, "FB2 file should not be identified as ZIP");
        }

        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Test FB2File FileInformation property provides correct file details")]
        public void FB2File_FileInformation_ProvidesCorrectDetails()
        {
            // Arrange
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act
            var fileInfo = file.FileInformation;

            // Assert
            Assert.IsNotNull(fileInfo, "FileInformation should not be null");
            Assert.IsTrue(fileInfo.Exists, "File should exist");
            Assert.AreEqual("Макиавелли Николо - Государь.fb2", fileInfo.Name, "File name should match");
            Assert.IsTrue(fileInfo.Length > 0, "File should have content");
        }

        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Test FB2File Metadata property contains description")]
        public void FB2File_Metadata_ContainsDescription()
        {
            // Arrange
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act
            var metadata = file.Metadata;

            // Assert
            Assert.IsNotNull(metadata, "Metadata should not be null");
            Assert.IsNotNull(metadata.Description, "Description should not be null");
            Assert.IsTrue(metadata.Description.Length > 0, "Description should have content");
        }

        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Test FB2File UpdateFileInfo changes FileInformation")]
        public void FB2File_UpdateFileInfo_ChangesFileInformation()
        {
            // Arrange
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");
            string originalName = file.FileInformation.Name;

            // Act
            file.UpdateFileInfo(@"TestFiles\Макиавелли Николо - Государь.fb2.zip");
            string newName = file.FileInformation.Name;

            // Assert
            Assert.AreNotEqual(originalName, newName, "File name should be different after update");
            Assert.AreEqual("Макиавелли Николо - Государь.fb2.zip", newName, "New file name should match");
        }

        [TestMethod]
        [TestCategory("FB2 File")]
        [Description("Test FB2File BookInternalEncoding is set via reflection")]
        public void FB2File_BookInternalEncoding_IsSet()
        {
            // Arrange
            FB2File file = new FB2File(@"TestFiles\Макиавелли Николо - Государь.fb2");

            // Act - access via reflection since it's protected internal
            var type = file.GetType();
            var property = type.GetProperty("BookInternalEncoding",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public);
            var value = property.GetValue(file, null) as string;

            // Assert
            Assert.IsNotNull(value, "BookInternalEncoding should be set");
            Assert.IsTrue(value.Length > 0, "BookInternalEncoding should not be empty");
        }
    }
}
