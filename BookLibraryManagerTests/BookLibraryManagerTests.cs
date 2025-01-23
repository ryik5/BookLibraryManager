using Xunit;
using BookLibraryManager.TestApp.ViewModel;
using Moq;
using BookLibraryManager.Common;

namespace BookLibraryManager.Tests;

/// <author>YR 2025-01-23</author>
public class BookLibraryTests
{
    [Fact]
    public void CreateNewLibrary_ShouldReturnNewLibrary()
    {
        // Arrange
        var manager = new BookLibraryManager.TestApp.ViewModel.BookLibraryManager();
        var libraryId = 1;

        // Act
        var library = manager.CreateNewLibrary(libraryId);

        // Assert
        Xunit.Assert.NotNull(library);
        Xunit.Assert.Equal(libraryId, library.Id);
    }

    [Fact]
    public void LoadLibrary_ShouldReturnTrue_WhenLibraryIsLoaded()
    {
        // Arrange
        var manager = new BookLibraryManager.TestApp.ViewModel.BookLibraryManager();
        var libraryId = 1;
        var mockLoader = new Mock<ILibraryLoader>();
        var mockLibrary = manager.CreateNewLibrary(libraryId);
        var path = "path/to/library";
        mockLoader.Setup(loader => loader.LoadLibrary(path, out mockLibrary)).Returns(true);

        // Act
        var result = manager.LoadLibrary(mockLoader.Object, path, out var library);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.Equal(mockLibrary, library);
    }

    [Fact]
    public void AddBook_ShouldAddBookToLibrary()
    {
        // Arrange
        var manager = new BookLibraryManager.TestApp.ViewModel.BookLibraryManager();
        var mockLibrary = new Mock<ILibrary>();
        var book = new Book { Id = 1, Author = "Author", Title = "Title", TotalPages = 100 };

        // Act
        manager.AddBook(mockLibrary.Object, book);

        // Assert
        mockLibrary.Verify(library => library.AddBook(book), Times.Once);
    }

    [Fact]
    public void RemoveBook_ShouldReturnTrue_WhenBookIsRemoved()
    {
        // Arrange
        var manager = new BookLibraryManager.TestApp.ViewModel.BookLibraryManager();
        var mockLibrary = new Mock<ILibrary>();
        var book = new Book { Id = 1, Author = "Author", Title = "Title", TotalPages = 100 };
        mockLibrary.Setup(library => library.RemoveBook(book)).Returns(true);

        // Act
        var result = manager.RemoveBook(mockLibrary.Object, book);

        // Assert
        Xunit.Assert.True(result);
    }

    [Fact]
    public void SortLibrary_ShouldSortBooksInLibrary()
    {
        // Arrange
        var manager = new BookLibraryManager.TestApp.ViewModel.BookLibraryManager();
        var mockLibrary = new Mock<ILibrary>();

        // Act
        manager.SortLibrary(mockLibrary.Object);

        // Assert
        mockLibrary.Verify(library => library.SortLibrary(), Times.Once);
    }

    [Fact]
    public void SaveLibrary_ShouldReturnTrue_WhenLibraryIsSaved()
    {
        // Arrange
        var manager = new BookLibraryManager.TestApp.ViewModel.BookLibraryManager();
        var mockKeeper = new Mock<ILibraryKeeper>();
        var mockLibrary = new Mock<ILibrary>();
        string path = "path/to/folder";
        mockKeeper.Setup(keeper => keeper.SaveLibrary(mockLibrary.Object, path)).Returns(true);

        // Act
        var result = manager.SaveLibrary(mockKeeper.Object, path, mockLibrary.Object);

        // Assert
        Xunit.Assert.True(result);
    }

    [Fact]
    public void FindBooksByBookElement_ShouldReturnMatchingBooks()
    {
        // Arrange
        var manager = new BookLibraryManager.TestApp.ViewModel.BookLibraryManager();
        var mockLibrary = new Mock<ILibrary>();
        var books = new List<Book>
        {
            new Book { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100 },
            new Book { Id = 2, Author = "Author2", Title = "Title2", TotalPages = 200 }
        };
        mockLibrary.Setup(library => library.FindBooksByBookElement(BookElementsEnum.Author, "Author1")).Returns(books.Where(b => b.Author == "Author1").ToList());

        // Act
        var result = manager.FindBooksByBookElement(mockLibrary.Object, BookElementsEnum.Author, "Author1");

        // Assert
        Xunit.Assert.Single(result);
        Xunit.Assert.Equal("Author1", result[0].Author);
    }

    [Fact]
    public void NewLibrary_SouldBeNotNull()
    {
        // Arrange
        var bookManager = new BookLibraryManager.TestApp.ViewModel.BookLibraryManager(); // Update this line to use the correct type
        var libraryId = 1;

        // Act
        var library = bookManager.CreateNewLibrary(libraryId);

        // Assert
        Xunit.Assert.NotNull(library.BookList);
        Xunit.Assert.Equal(library.Id, libraryId);
    }

    [Fact]
    public void AddBook_ShouldAddBookToLibrary1()
    {
        // Arrange
        var bookManager = new BookLibraryManager.TestApp.ViewModel.BookLibraryManager(); // Update this line to use the correct type
        var library = bookManager.CreateNewLibrary(1);
        var addedBook = new Book() { Id = 1, Title = "Book 1", Author = "Author 1", TotalPages = 1 };

        // Act
        bookManager.AddBook(library, addedBook);

        // Assert
        var lastBook = library.BookList.Last();
        Xunit.Assert.Equal(lastBook, addedBook);
    }
}
