using FB2Toolbox;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FB2ToolboxUnitTest
{
    [TestClass]
    public class ConfigTests
    {
        [TestClass]
        public class GenreSubstitutionElementTests
        {
            [TestMethod]
            [TestCategory("Config")]
            [Description("Test GenreSubstitutionElement properties")]
            public void GenreSubstitutionElement_Properties_Work()
            {
                // Arrange
                var element = new GenreSubstitutionElement();

                // Act
                element.From = "sf";
                element.To = "Science Fiction";

                // Assert
                Assert.AreEqual("sf", element.From);
                Assert.AreEqual("Science Fiction", element.To);
            }

            [TestMethod]
            [TestCategory("Config")]
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
                Assert.AreEqual("Science Fiction (sf)", result);
            }

            [TestMethod]
            [TestCategory("Config")]
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
                Assert.IsTrue(result > 0, "Science Fiction should come after Fantasy alphabetically");
            }

            [TestMethod]
            [TestCategory("Config")]
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
                Assert.AreEqual(0, result);
            }
        }

        [TestClass]
        public class EncodingElementTests
        {
            [TestMethod]
            [TestCategory("Config")]
            [Description("Test EncodingElement Name property")]
            public void EncodingElement_Name_Works()
            {
                // Arrange
                var element = new EncodingElement();

                // Act
                element.Name = "UTF-8";

                // Assert
                Assert.AreEqual("UTF-8", element.Name);
            }
        }

        [TestClass]
        public class RenameProfileElementTests
        {
            [TestMethod]
            [TestCategory("Config")]
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
                Assert.AreEqual("Test Profile", element.Name);
                Assert.AreEqual(@"(AuthorLastName)\(SequenceName)", element.Path);
                Assert.AreEqual("(AuthorLastName) - (Title)", element.FileName);
            }

            [TestMethod]
            [TestCategory("Config")]
            [Description("Test RenameProfileElement CharacterSubstitution collection")]
            public void RenameProfileElement_CharacterSubstitution_IsNotNull()
            {
                // Arrange
                var element = new RenameProfileElement();

                // Act
                var substitutions = element.CharacterSubstitution;

                // Assert
                Assert.IsNotNull(substitutions);
            }
        }

        [TestClass]
        public class CharacterSubstitutionElementTests
        {
            [TestMethod]
            [TestCategory("Config")]
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
                Assert.AreEqual("?", element.From);
                Assert.AreEqual("_", element.To);
                Assert.AreEqual(2, element.Repeat);
            }
        }

        [TestClass]
        public class CommandElementTests
        {
            [TestMethod]
            [TestCategory("Config")]
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
                Assert.AreEqual("Open with Notepad", element.Name);
                Assert.AreEqual("notepad.exe", element.FileName);
                Assert.AreEqual("{0}", element.Arguments);
                Assert.IsFalse(element.CreateNoWindow);
                Assert.AreEqual(".fb2", element.OnlyWithExtension);
                Assert.IsTrue(element.WaitAndReload);
            }
        }

        [TestClass]
        public class GenresCollectionTests
        {
            [TestMethod]
            [TestCategory("Config")]
            [Description("Test GenresCollection FindSubstitution returns original if not found")]
            public void GenresCollection_FindSubstitution_ReturnsOriginalWhenNotFound()
            {
                // Arrange
                var collection = new GenresCollection();

                // Act
                string result = collection.FindSubstitution("unknown_genre");

                // Assert
                Assert.AreEqual("unknown_genre", result);
            }

            [TestMethod]
            [TestCategory("Config")]
            [Description("Test GenresCollection FindSubstitution returns empty for null")]
            public void GenresCollection_FindSubstitution_ReturnsEmptyForNull()
            {
                // Arrange
                var collection = new GenresCollection();

                // Act
                string result = collection.FindSubstitution(null);

                // Assert
                Assert.AreEqual(string.Empty, result);
            }

            [TestMethod]
            [TestCategory("Config")]
            [Description("Test GenresCollection FindSubstitution returns empty for empty string")]
            public void GenresCollection_FindSubstitution_ReturnsEmptyForEmptyString()
            {
                // Arrange
                var collection = new GenresCollection();

                // Act
                string result = collection.FindSubstitution(string.Empty);

                // Assert
                Assert.AreEqual(string.Empty, result);
            }
        }
    }
}
