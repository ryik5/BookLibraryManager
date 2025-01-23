using BookLibraryManager.Common;
using Moq;
using Xunit;

public class LibrarySearcherTests
{
    private readonly Mock<ILibraryBookLocator> _librarySearcherMock;

    public LibrarySearcherTests()
    {
        _librarySearcherMock = new Mock<ILibraryBookLocator>();
    }

    [Fact]
    public void FindBooksByBookElement_ShouldReturnBooks_WhenBooksMatchCriteria()
    {
        // Arrange
        var bookElement = BookElementsEnum.Author;
        var partOfElement = "John Doe";
        var expectedBooks = new List<Book>
        {
            new Book { Id = 1, Author = "John Doe", Title = "Book 1", TotalPages = 100 },
            new Book { Id = 2, Author = "John Doe", Title = "Book 2", TotalPages = 200 }
        };

        _librarySearcherMock.Setup(x => x.FindBooksByBookElement(bookElement, partOfElement))
                            .Returns(expectedBooks);

        // Act
        var result = _librarySearcherMock.Object.FindBooksByBookElement(bookElement, partOfElement);

        // Assert
        Xunit.Assert.Equal(expectedBooks, result);
    }

    [Fact]
    public void FindBooksByBookElement_ShouldReturnEmptyList_WhenNoBooksMatchCriteria()
    {
        // Arrange
        var bookElement = BookElementsEnum.Title;
        var partOfElement = "Nonexistent Book";
        var expectedBooks = new List<Book>();

        _librarySearcherMock.Setup(x => x.FindBooksByBookElement(bookElement, partOfElement))
                            .Returns(expectedBooks);

        // Act
        var result = _librarySearcherMock.Object.FindBooksByBookElement(bookElement, partOfElement);

        // Assert
        Xunit.Assert.Empty(result);
    }
}
