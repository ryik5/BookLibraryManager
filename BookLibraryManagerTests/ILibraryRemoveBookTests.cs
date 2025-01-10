using BookLibraryManager.Common;
using Xunit;

namespace BookLibraryManager.Tests
{
    public class ILibraryRemoveBookTests
    {
        private readonly ILibraryRemoveBook _libraryRemoveBook;

        public ILibraryRemoveBookTests()
        {
            _libraryRemoveBook = new MockLibraryRemoveBook();
        }

        [Fact]
        public void RemoveBook_ValidBook_ReturnsTrue()
        {
            // Arrange
            var book = new Book { Id = 1, Author = "Author", Title = "Title", PageNumber = 1 };

            // Act
            var result = _libraryRemoveBook.RemoveBook(book);

            // Assert
            Xunit.Assert.True(result);
        }

        [Fact]
        public void RemoveBook_InvalidBook_ReturnsFalse()
        {
            // Arrange
            var book = new Book { Id = 2, Author = "Unknown", Title = "Unknown", PageNumber = 1 };

            // Act
            var result = _libraryRemoveBook.RemoveBook(book);

            // Assert
            Xunit.Assert.False(result);
        }

        private class MockLibraryRemoveBook : ILibraryRemoveBook
        {
            public bool RemoveBook(Book book)
            {
                if (book.Id == 1)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
