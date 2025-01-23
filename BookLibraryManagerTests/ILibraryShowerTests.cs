using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

/// <author>YR 2025-01-09</author>
public class ILibraryShowerTests
{
    [Fact]
    public void GetFirstBooks_ShouldReturnCorrectNumberOfBooks()
    {
        // Arrange
        var mockLibraryShower = new Mock<ILibraryShower>();
        var books = new List<Book>
        {
            new() { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100 },
            new() { Id = 2, Author = "Author2", Title = "Title2", TotalPages = 200 }
        };
        mockLibraryShower.Setup(m => m.GetFirstBooks(2)).Returns(books);

        // Act
        var result = mockLibraryShower.Object.GetFirstBooks(2);

        // Assert
        Xunit.Assert.Equal(2, result.Count);
        Xunit.Assert.Equal("Author1", result[0].Author);
        Xunit.Assert.Equal("Author2", result[1].Author);
    }

    [Fact]
    public void NumberOfBooks_ShouldReturnCorrectNumber()
    {
        // Arrange
        var mockLibraryShower = new Mock<ILibraryShower>();
        mockLibraryShower.Setup(m => m.NumberOfBooks).Returns(5);

        // Act
        var result = mockLibraryShower.Object.NumberOfBooks;

        // Assert
        Xunit.Assert.Equal(5, result);
    }
}
