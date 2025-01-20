using System.Collections.ObjectModel;
using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

public class BookLibraryManagerTests
{
    private readonly Mock<IBookListLoadable> _mockLoader;
    private readonly Mock<IBookListSaveable> _mockSaver;
    private readonly Mock<ILibrary> _mockLibrary;
    private readonly BookLibraryManager _libraryManager;

    public BookLibraryManagerTests()
    {
        // Setup
        _mockLoader = new Mock<IBookListLoadable>();
        _mockSaver = new Mock<IBookListSaveable>();
        _mockLibrary = new Mock<ILibrary>();
        _libraryManager = new BookLibraryManager();
    }

    [Fact]
    public void CreateNewLibrary_ShouldReturnNewLibraryInstance()
    {
        // Arrange
        var idLibrary = 1;
        _mockLibrary.Setup(x => x.Id).Returns(idLibrary);

        // Act
        var library = _libraryManager.CreateNewLibrary(idLibrary);

        // Assert
        Xunit.Assert.NotNull(library);
        Xunit.Assert.Equal(idLibrary, library.Id);
    }

    [Fact]
    public void LoadLibrary_ShouldReturnTrue_WhenLibraryIsLoadedSuccessfully()
    {
        // Arrange
        var path = "pathToLibrary";
        var mockLoader = new Mock<IBookListLoadable>();
        mockLoader.Setup(loader => loader.LoadLibrary(It.IsAny<string>(), out It.Ref<ILibrary>.IsAny)).Returns(true);

        var manager = new BookLibraryManager();
        ILibrary library;

        // Act
        var result = manager.LoadLibrary(mockLoader.Object, path, out library);

        // Assert
        Xunit.Assert.True(result);
    }

    [Fact]
    public void LoadLibrary_ShouldReturnFalse_WhenLibraryIsNotLoaded()
    {
        // Arrange
        var path = "invalidPathToLibrary";

        // Act
        var result = _libraryManager.LoadLibrary(_mockLoader.Object, path, out It.Ref<ILibrary>.IsAny);

        // Assert
        Xunit.Assert.False(result);
    }

    [Fact]
    public void AddBook_ShouldAddBookToLibrary()
    {
        // Arrange
        var book = new Book { Id = 1, Author = "Author", Title = "Title", PageNumber = 1 };
        var library = _libraryManager.CreateNewLibrary(1);

        // Act
        _libraryManager.AddBook(library, book);

        // Assert
        var result = library.BookList.Last();
        Xunit.Assert.Equal(result, book);
    }

    [Fact]
    public void RemoveBook_ShouldReturnTrue_WhenBookIsRemoved()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var book = new Book { Id = 1, Author = "Author", Title = "Title", PageNumber = 1 };
        mockLibrary.Setup(library => library.RemoveBook(book)).Returns(true);

        var manager = new BookLibraryManager();

        // Act
        var result = manager.RemoveBook(mockLibrary.Object, book);

        // Assert
        Xunit.Assert.True(result);
    }

    [Fact]
    public void RemoveBook_ShouldReturnFalse_WhenBookIsNotRemoved()
    {
        // Arrange
        var book = new Book { Id = 1, Author = "Author", Title = "Title", PageNumber = 100 };

        // Act
        var result = _libraryManager.RemoveBook(_mockLibrary.Object, book);

        // Assert
        Xunit.Assert.False(result);
    }

    [Fact]
    public void SortLibrary_ShouldSortBooksInLibrary()
    {
        // Arrange
        _mockLibrary.Setup(library => library.SortLibrary());

        // Act
        _libraryManager.SortLibrary(_mockLibrary.Object);

        // Assert
    }

    [Fact]
    public void FindBooksByTitle_ShouldReturnBooksContainingTitlePart()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var books = new ObservableCollection<Book>
        {
            new() { Id = 1, Title = "C# Programming", Author = "Author1", PageNumber = 300 },
            new() { Id = 2, Title = "Learn C#", Author = "Author2", PageNumber = 250 },
            new() { Id = 3, Title = "Java Programming", Author = "Author3", PageNumber = 400 }
        };
        mockLibrary.Setup(l => l.BookList).Returns(books);

        var manager = new BookLibraryManager();

        // Act
        var result = manager.FindBooksByTitle(mockLibrary.Object, "C#");

        // Assert
        Xunit.Assert.Equal(2, result.Count);
        Xunit.Assert.Contains(result, b => b.Title == "C# Programming");
        Xunit.Assert.Contains(result, b => b.Title == "Learn C#");
    }

    [Fact]
    public void FindBooksByTitle_ShouldReturnEmptyList_WhenNoTitleContainsPartOfTitle()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var books = new ObservableCollection<Book>
        {
            new() { Id = 1, Title = "C# Programming", Author = "Author1", PageNumber = 300 },
            new() { Id = 2, Title = "Learn C#", Author = "Author2", PageNumber = 250 },
            new() { Id = 3, Title = "Java Programming", Author = "Author3", PageNumber = 400 }
        };
        mockLibrary.Setup(l => l.BookList).Returns(books);

        var manager = new BookLibraryManager();

        // Act
        var result = manager.FindBooksByTitle(mockLibrary.Object, "Python");

        // Assert
        Xunit.Assert.Empty(result);
    }

    [Fact]
    public void SaveLibrary_ShouldReturnTrue_WhenLibraryIsSavedSuccessfully()
    {
        string path = "path_to_library";
        _mockSaver.Setup(saver => saver.SaveLibrary(_mockLibrary.Object, path)).Returns(true);

        bool result = _libraryManager.SaveLibrary(_mockSaver.Object, path, _mockLibrary.Object);

        Xunit.Assert.True(result);
    }

    [Fact]
    public void SaveLibrary_ShouldReturnFalse_WhenLibraryIsNotSaved()
    {
        string path = "path_to_library";
        _mockSaver.Setup(saver => saver.SaveLibrary(_mockLibrary.Object, path)).Returns(false);

        var result = _libraryManager.SaveLibrary(_mockSaver.Object, path, _mockLibrary.Object);

        Xunit.Assert.False(result);
    }
}
