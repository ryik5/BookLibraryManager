using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

/// <author>YR 2025-01-09</author>
public class ILibraryAddBookTests
{
    [Fact]
    public void AddBook_ShouldAddBookToLibrary()
    {
        // Arrange
        var mockLibrary = new Mock<ILibraryAddBook>();
        var book = new Book
        {
            Id = 1,
            Author = "Author Name",
            Title = "Book Title",
            PageNumber = 1
        };

        // Act
        mockLibrary.Object.AddBook(book);

        // Assert
        mockLibrary.Verify(lib => lib.AddBook(It.Is<Book>(b => b.Id == book.Id && b.Author == book.Author && b.Title == book.Title && b.PageNumber == book.PageNumber)), Times.Once);
    }
}
