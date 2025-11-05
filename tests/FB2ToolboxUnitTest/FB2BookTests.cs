using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FB2Toolbox.Entities;

namespace FB2ToolboxUnitTest
{
    [TestClass]
    public class FB2BookTests
    {
        [TestMethod]
        [TestCategory("FB2Book")]
        [Description("Test FB2Book default constructor initializes properties correctly")]
        public void FB2Book_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var book = new FB2Book();

            // Assert
            Assert.AreEqual(string.Empty, book.BookFile);
            Assert.AreEqual(string.Empty, book.BookAuthorFirstName);
            Assert.AreEqual(string.Empty, book.BookAuthorLastName);
            Assert.AreEqual(string.Empty, book.BookAuthorMiddleName);
            Assert.AreEqual(string.Empty, book.BookEncoding);
            Assert.AreEqual(string.Empty, book.BookTitle);
            Assert.AreEqual(string.Empty, book.BookSequenceName);
            Assert.IsNull(book.BookSequenceNr);
            Assert.AreEqual(string.Empty, book.BookLang);
        }

        [TestMethod]
        [TestCategory("FB2Book")]
        [Description("Test FB2Book CompareTo by LastName")]
        public void FB2Book_CompareTo_ComparesByLastName()
        {
            // Arrange
            var book1 = new FB2Book { BookAuthorLastName = "Ivanov" };
            var book2 = new FB2Book { BookAuthorLastName = "Petrov" };

            // Act
            int result = book1.CompareTo(book2);

            // Assert
            Assert.IsTrue(result < 0, "Ivanov should come before Petrov");
        }

        [TestMethod]
        [TestCategory("FB2Book")]
        [Description("Test FB2Book CompareTo by FirstName when LastName is equal")]
        public void FB2Book_CompareTo_ComparesByFirstNameWhenLastNameEqual()
        {
            // Arrange
            var book1 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan"
            };
            var book2 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Petr"
            };

            // Act
            int result = book1.CompareTo(book2);

            // Assert
            Assert.IsTrue(result < 0, "Ivan should come before Petr");
        }

        [TestMethod]
        [TestCategory("FB2Book")]
        [Description("Test FB2Book CompareTo by SequenceName when author names are equal")]
        public void FB2Book_CompareTo_ComparesBySequenceNameWhenAuthorsEqual()
        {
            // Arrange
            var book1 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan",
                BookSequenceName = "Series A"
            };
            var book2 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan",
                BookSequenceName = "Series B"
            };

            // Act
            int result = book1.CompareTo(book2);

            // Assert
            Assert.IsTrue(result < 0, "Series A should come before Series B");
        }

        [TestMethod]
        [TestCategory("FB2Book")]
        [Description("Test FB2Book CompareTo by SequenceNr when everything else is equal")]
        public void FB2Book_CompareTo_ComparesBySequenceNrWhenOthersEqual()
        {
            // Arrange
            var book1 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan",
                BookSequenceName = "Series A",
                BookSequenceNr = 1
            };
            var book2 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan",
                BookSequenceName = "Series A",
                BookSequenceNr = 2
            };

            // Act
            int result = book1.CompareTo(book2);

            // Assert
            Assert.IsTrue(result < 0, "Book 1 should come before Book 2");
        }

        [TestMethod]
        [TestCategory("FB2Book")]
        [Description("Test FB2Book CompareTo by Title when all else is equal")]
        public void FB2Book_CompareTo_ComparesByTitleWhenAllElseEqual()
        {
            // Arrange
            var book1 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan",
                BookSequenceName = "Series A",
                BookSequenceNr = 1,
                BookTitle = "Book A"
            };
            var book2 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan",
                BookSequenceName = "Series A",
                BookSequenceNr = 1,
                BookTitle = "Book B"
            };

            // Act
            int result = book1.CompareTo(book2);

            // Assert
            Assert.IsTrue(result < 0, "Book A should come before Book B");
        }

        [TestMethod]
        [TestCategory("FB2Book")]
        [Description("Test FB2Book CompareTo returns 0 for equal books")]
        public void FB2Book_CompareTo_ReturnsZeroForEqualBooks()
        {
            // Arrange
            var book1 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan",
                BookSequenceName = "Series A",
                BookSequenceNr = 1,
                BookTitle = "Book A"
            };
            var book2 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan",
                BookSequenceName = "Series A",
                BookSequenceNr = 1,
                BookTitle = "Book A"
            };

            // Act
            int result = book1.CompareTo(book2);

            // Assert
            Assert.AreEqual(0, result, "Identical books should return 0");
        }

        [TestMethod]
        [TestCategory("FB2Book")]
        [Description("Test FB2Book CompareTo with null sequence numbers")]
        public void FB2Book_CompareTo_HandlesNullSequenceNumbers()
        {
            // Arrange
            var book1 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan",
                BookSequenceName = "Series A",
                BookSequenceNr = null
            };
            var book2 = new FB2Book 
            { 
                BookAuthorLastName = "Ivanov",
                BookAuthorFirstName = "Ivan",
                BookSequenceName = "Series A",
                BookSequenceNr = 1
            };

            // Act
            int result = book1.CompareTo(book2);

            // Assert
            Assert.IsTrue(result < 0, "Null sequence number should come before numbered");
        }

        [TestMethod]
        [TestCategory("FB2Book")]
        [Description("Test FB2Book CompareTo throws InvalidCastException for non-IBook object")]
        [ExpectedException(typeof(InvalidCastException))]
        public void FB2Book_CompareTo_ThrowsExceptionForInvalidType()
        {
            // Arrange
            var book = new FB2Book();
            var invalidObject = "not a book";

            // Act
            book.CompareTo(invalidObject);

            // Assert - exception expected
        }

        [TestMethod]
        [TestCategory("FB2Book")]
        [Description("Test FB2Book CompareTo is case insensitive")]
        public void FB2Book_CompareTo_IsCaseInsensitive()
        {
            // Arrange
            var book1 = new FB2Book { BookAuthorLastName = "ivanov" };
            var book2 = new FB2Book { BookAuthorLastName = "IVANOV" };

            // Act
            int result = book1.CompareTo(book2);

            // Assert
            Assert.AreEqual(0, result, "Comparison should be case insensitive");
        }
    }
}
