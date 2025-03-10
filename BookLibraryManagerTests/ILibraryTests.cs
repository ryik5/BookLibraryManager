using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

/// <summary>
/// Unit tests for the ILibrary interface.
/// </summary>
/// <author>YR 2025-01-23</author>
public class ILibraryTests
{
    private readonly Mock<IBookManageable> _libraryMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="ILibraryTests"/> class.
    /// </summary>
    public ILibraryTests()
    {
        _libraryMock = new Mock<IBookManageable>();
    }

    /// <summary>
    /// Tests that FindBooksByBookElement returns books when books match the criteria.
    /// </summary>
    [Fact]
    public void FindBooksByBookElement_ShouldReturnBooks_WhenBooksMatchCriteria()
    {
        // Arrange
        var bookElement = EBibliographicKindInformation.Author;
        var partOfElement = "John Doe";
        var expectedBooks = new List<Book>
        {
            new() { Id = 1, Author = "John Doe", Title = "Book 1", TotalPages = 100 },
            new() { Id = 2, Author = "John Doe", Title = "Book 2", TotalPages = 200 }
        };

        _libraryMock.Setup(x => x.FindBooksByKind(bookElement, partOfElement))
                            .Returns(expectedBooks);

        // Act
        var result = _libraryMock.Object.FindBooksByKind(bookElement, partOfElement);

        // Assert
        Xunit.Assert.Equal(expectedBooks, result);
    }

    /// <summary>
    /// Tests that FindBooksByBookElement returns an empty list when no books match the criteria.
    /// </summary>
    [Fact]
    public void FindBooksByBookElement_ShouldReturnEmptyList_WhenNoBooksMatchCriteria()
    {
        // Arrange
        var bookElement = EBibliographicKindInformation.Title;
        var partOfElement = "Nonexistent Book";
        var expectedBooks = new List<Book>();

        _libraryMock.Setup(x => x.FindBooksByKind(bookElement, partOfElement))
                            .Returns(expectedBooks);

        // Act
        var result = _libraryMock.Object.FindBooksByKind(bookElement, partOfElement);

        // Assert
        Xunit.Assert.Empty(result);
    }

    /// <summary>
    /// Tests that RemoveBook returns true when a valid book is removed.
    /// </summary>
    [Fact]
    public void RemoveBook_ValidBook_ReturnsTrue()
    {
        // Arrange
        var book = new Book { Id = 1, Author = "Author", Title = "Title", TotalPages = 1 };
        _libraryMock.Setup(m => m.TryRemoveBook(book)).Returns(true);

        // Act
        var result = _libraryMock.Object.TryRemoveBook(book);

        // Assert
        Xunit.Assert.True(result);
    }

    /// <summary>
    /// Tests that RemoveBook returns false when an invalid book is removed.
    /// </summary>
    [Fact]
    public void RemoveBook_InvalidBook_ReturnsFalse()
    {
        // Arrange
        var book = new Book { Id = 2, Author = "Unknown", Title = "Unknown", TotalPages = 1 };

        // Act
        var result = _libraryMock.Object.TryRemoveBook(book);

        // Assert
        Xunit.Assert.False(result);
    }

    /// <summary>
    /// Tests that AddBook adds a book to the library.
    /// </summary>
    [Fact]
    public void AddBook_ShouldAddBookToLibrary()
    {
        // Arrange
        var book = new Book
        {
            Id = 1,
            Author = "Author Name",
            Title = "Book Title",
            TotalPages = 1
        };

        // Act
        _libraryMock.Object.AddBook(book);

        // Assert
        _libraryMock.Verify(lib => lib.AddBook(It.Is<Book>(b => b.Id == book.Id && b.Author == book.Author && b.Title == book.Title && b.TotalPages == book.TotalPages)), Times.Once);
    }
}
