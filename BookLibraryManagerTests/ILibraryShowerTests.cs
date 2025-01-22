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
            new Book { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100 },
            new Book { Id = 2, Author = "Author2", Title = "Title2", TotalPages = 200 }
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
    public void ShowFistBooks_ShouldReturnCorrectString()
    {
        // Arrange
        var mockLibraryShower = new Mock<ILibraryShower>();
        mockLibraryShower.Setup(m => m.ShowFistBooks(2)).Returns("Title1, Title2");

        // Act
        var result = mockLibraryShower.Object.ShowFistBooks(2);

        // Assert
        Xunit.Assert.Equal("Title1, Title2", result);
    }

    [Fact]
    public void ShowLastBooks_ShouldReturnCorrectString()
    {
        // Arrange
        var mockLibraryShower = new Mock<ILibraryShower>();
        mockLibraryShower.Setup(m => m.ShowLastBooks(2)).Returns("Title3, Title4");

        // Act
        var result = mockLibraryShower.Object.ShowLastBooks(2);

        // Assert
        Xunit.Assert.Equal("Title3, Title4", result);
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
