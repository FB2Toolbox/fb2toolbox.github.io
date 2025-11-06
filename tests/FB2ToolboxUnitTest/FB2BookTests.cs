using System;
using System.ComponentModel;
using FB2Toolbox.Entities;
using AwesomeAssertions;
using Xunit;

namespace FB2ToolboxUnitTest
{
    public class FB2BookTests
    {
        [Fact]
        [Trait("Category", "FB2Book")]
        [Description("Test FB2Book default constructor initializes properties correctly")]
        public void FB2Book_DefaultConstructor_InitializesProperties()
        {
            // Arrange & Act
            var book = new FB2Book();

            // Assert
            book.BookFile.Should().BeEmpty();
            book.BookAuthorFirstName.Should().BeEmpty();
            book.BookAuthorLastName.Should().BeEmpty();
            book.BookAuthorMiddleName.Should().BeEmpty();
            book.BookEncoding.Should().BeEmpty();
            book.BookTitle.Should().BeEmpty();
            book.BookSequenceName.Should().BeEmpty();
            book.BookSequenceNr.Should().BeNull();
            book.BookLang.Should().BeEmpty();
        }

        [Fact]
        [Trait("Category", "FB2Book")]
        [Description("Test FB2Book CompareTo by LastName")]
        public void FB2Book_CompareTo_ComparesByLastName()
        {
            // Arrange
            var book1 = new FB2Book { BookAuthorLastName = "Ivanov" };
            var book2 = new FB2Book { BookAuthorLastName = "Petrov" };

            // Act
            int result = book1.CompareTo(book2);

            // Assert
            result.Should().BeLessThan(0);
        }

        [Fact]
        [Trait("Category", "FB2Book")]
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
            result.Should().BeLessThan(0);
        }

        [Fact]
        [Trait("Category", "FB2Book")]
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
            result.Should().BeLessThan(0);
        }

        [Fact]
        [Trait("Category", "FB2Book")]
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
            result.Should().BeLessThan(0);
        }

        [Fact]
        [Trait("Category", "FB2Book")]
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
            result.Should().BeNegative();
        }

        [Fact]
        [Trait("Category", "FB2Book")]
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
            result.Should().Be(0);
        }

        [Fact]
        [Trait("Category", "FB2Book")]
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
            result.Should().BeNegative();
        }

        [Fact]
        [Trait("Category", "FB2Book")]
        [Description("Test FB2Book CompareTo throws InvalidCastException for non-IBook object")]
        public void FB2Book_CompareTo_ThrowsExceptionForInvalidType()
        {
            // Arrange
            var book = new FB2Book();
            var invalidObject = "not a book";

            // Act
            Action act = () => book.CompareTo(invalidObject);

            // Assert - exception expected
            act.Should().Throw<InvalidCastException>();
        }

        [Fact]
        [Trait("Category", "FB2Book")]
        [Description("Test FB2Book CompareTo is case insensitive")]
        public void FB2Book_CompareTo_IsCaseInsensitive()
        {
            // Arrange
            var book1 = new FB2Book { BookAuthorLastName = "ivanov" };
            var book2 = new FB2Book { BookAuthorLastName = "IVANOV" };

            // Act
            int result = book1.CompareTo(book2);

            // Assert
            result.Should().Be(0);
        }
    }
}
