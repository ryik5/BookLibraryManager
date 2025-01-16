using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

public class IBookLibraryManageableTests
{
    private readonly Mock<IBookListLoadable> _mockLoader;
    private readonly Mock<IBookListSaveable> _mockSaver;
    private readonly Mock<ILibrary> _mockLibrary;
    private readonly Mock<IBookLibraryManageable> _libraryManager;

    public IBookLibraryManageableTests()
    {
        // Setup
        _mockLoader = new Mock<IBookListLoadable>();
        _mockSaver = new Mock<IBookListSaveable>();
        _mockLibrary = new Mock<ILibrary>();
        _libraryManager = new Mock<IBookLibraryManageable>();
    }

    [Fact]
    public void CreateNewLibrary_ShouldReturnNewLibraryInstance()
    {
        // Arrange
        int idLibrary = 1;
        _mockLibrary.Setup(x => x.Id).Returns(idLibrary);
        _libraryManager.Setup(manager => manager.CreateNewLibrary(idLibrary)).Returns(_mockLibrary.Object);

        // Act
        var library = _libraryManager.Object.CreateNewLibrary(idLibrary);

        // Assert
        Xunit.Assert.NotNull(library);
        Xunit.Assert.Equal(idLibrary, library.Id);
    }

    [Fact]
    public void LoadLibrary_ShouldReturnTrue_WhenLibraryIsLoadedSuccessfully()
    {
        // Arrange
        var path = "pathToLibrary";
        _libraryManager.Setup(manager => manager.LoadLibrary(_mockLoader.Object, path, out It.Ref<ILibrary>.IsAny)).Returns(true);

        // Act
        var result = _libraryManager.Object.LoadLibrary(_mockLoader.Object, path, out It.Ref<ILibrary>.IsAny);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.IsAssignableFrom<ILibrary>(_libraryManager.Object);
    }

    [Fact]
    public void LoadLibrary_ShouldReturnFalse_WhenLibraryIsNotLoaded()
    {
        // Arrange
        var path = "invalidPathToLibrary";
        _libraryManager.Setup(x => x.LoadLibrary(_mockLoader.Object, path, out It.Ref<ILibrary>.IsAny)).Returns(false);

        // Act
        var result = _libraryManager.Object.LoadLibrary(_mockLoader.Object, path, out It.Ref<ILibrary>.IsAny);

        // Assert
        Xunit.Assert.False(result);
    }

    [Fact]
    public void AddBook_ShouldAddBookToLibrary()
    {
        // Arrange
        var book = new Book { Id = 1, Author = "Author", Title = "Title", PageNumber = 1 };

        // Act
        _libraryManager.Object.AddBook(_mockLibrary.Object, book);

        // Assert
        _libraryManager.Verify(manager => manager.AddBook(_mockLibrary.Object, book), Times.Once);
    }


    [Fact]
    public void RemoveBook_ShouldReturnTrue_WhenBookIsRemoved()
    {
        // Arrange
        var book = new Book { Id = 1, Author = "Author", Title = "Title", PageNumber = 100 };
        _libraryManager.Setup(manager => manager.RemoveBook(_mockLibrary.Object, book)).Returns(true);

        // Act
        bool result = _libraryManager.Object.RemoveBook(_mockLibrary.Object, book);
        // Assert

        // Assert
        Xunit.Assert.True(result);
    }

    [Fact]
    public void RemoveBook_ShouldReturnFalse_WhenBookIsNotRemoved()
    {
        // Arrange
        var book = new Book { Id = 1, Author = "Author", Title = "Title", PageNumber = 100 };
        _libraryManager.Setup(manager => manager.RemoveBook(_mockLibrary.Object, book)).Returns(false);

        // Act
        bool result = _libraryManager.Object.RemoveBook(_mockLibrary.Object, book);

        // Assert
        Xunit.Assert.False(result);
    }

    [Fact]
    public void SortLibrary_ShouldSortBooksInLibrary()
    {
        // Arrange
        _mockLibrary.Setup(library => library.SortLibrary());
        _libraryManager.Setup(manager => manager.SortLibrary(_mockLibrary.Object));

        // Act
        _libraryManager.Object.SortLibrary(_mockLibrary.Object);

        // Assert
        _libraryManager.Verify(manager => manager.SortLibrary(_mockLibrary.Object), Times.Once);
    }

    [Fact]
    public void FindBooksByTitle_ShouldReturnBooksContainingTitlePart()
    {
        // Arrange
        var partOfTitle = "Title";
        var books = new List<Book>
        {
            new Book { Id = 1, Author = "Author1", Title = "Title1", PageNumber = 100 },
            new Book { Id = 2, Author = "Author2", Title = "Title2", PageNumber = 200 }
        };
        var expectedNumberOfBooks = 2;
        _mockLibrary.Setup(library => library.GetAllBooks()).Returns(books);
        _libraryManager.Setup(manager => manager.FindBooksByTitle(_mockLibrary.Object, partOfTitle)).Returns(books);

        // Act
        var currentNumberOfBooks = _libraryManager.Object.FindBooksByTitle(_mockLibrary.Object, partOfTitle).Count;

        // Assert
        Xunit.Assert.Equal(expectedNumberOfBooks, currentNumberOfBooks);
    }

    [Fact]
    public void SaveLibrary_ShouldReturnTrue_WhenLibraryIsSavedSuccessfully()
    {
        string path = "path/to/library";
        _mockSaver.Setup(saver => saver.SaveLibrary(_mockLibrary.Object, path)).Returns(true);
        _libraryManager.Setup(manager => manager.SaveLibrary(_mockSaver.Object, path, _mockLibrary.Object)).Returns(true);

        bool result = _libraryManager.Object.SaveLibrary(_mockSaver.Object, path, _mockLibrary.Object);

        Xunit.Assert.True(result);
    }

    [Fact]
    public void SaveLibrary_ShouldReturnFalse_WhenLibraryIsNotSaved()
    {
        string path = "path/to/library";
        _mockSaver.Setup(saver => saver.SaveLibrary(_mockLibrary.Object, path)).Returns(false);

        bool result = _libraryManager.Object.SaveLibrary(_mockSaver.Object, path, _mockLibrary.Object);

        Xunit.Assert.False(result);
    }
}
