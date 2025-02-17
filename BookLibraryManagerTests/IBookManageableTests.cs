using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

/// <author>YR 2025-01-09</author>
public class IBookManageableTests
{
    private readonly Mock<IBookManageable> _bookManagerMock;
    private readonly Mock<ILibraryManageable> _libraryManagerMock;
    private readonly Mock<ILibrary> _libraryMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="ILibraryTests"/> class.
    /// </summary>
    public IBookManageableTests()
    {
        _bookManagerMock = new Mock<IBookManageable>();
        _libraryManagerMock = new Mock<ILibraryManageable>();
    }


    /// <summary>
    /// Tests if a new library is initialized correctly.
    /// </summary>
    [Fact]
    public void CreateNewLibrary_ShouldInitializeLibrary()
    {
        // Arrange
        var libraryId = 1;
        _libraryManagerMock.Setup(x => x.Library.Id).Returns(libraryId);

        // Act
        var result = _libraryManagerMock.Object.Library.Id;

        // Assert
        Xunit.Assert.Equal(libraryId, result);
    }

    /// <summary>
    /// Tests if a library is loaded correctly from a file.
    /// </summary>
    [Fact]
    public void LoadLibrary_ShouldLoadLibraryFromFile()
    {
        // Arrange
        var mockLibraryLoader = new Mock<ILibraryLoader>();
        ILibrary library = new Library
        {
            Id = 1,
            BookList =
            [
                new() { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100, PublishDate = 2020 }
            ]
        };
        mockLibraryLoader.Setup(loader => loader.TryLoadLibrary(It.IsAny<string>(), out library)).Returns(true);

        // Act
        var result = _libraryManagerMock.Object.TryLoadLibrary(mockLibraryLoader.Object, "path/to/file");

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.Equal(1, library.Id);
        Xunit.Assert.Single(library.BookList);
    }

    /// <summary>
    /// Tests if a library is saved correctly to a file.
    /// </summary>
    [Fact]
    public void SaveLibrary_ShouldSaveLibraryToFile()
    {
        // Arrange
        var mockLibraryKeeper = new Mock<ILibraryKeeper>();
        mockLibraryKeeper.Setup(keeper => keeper.TrySaveLibrary(It.IsAny<ILibrary>(), It.IsAny<string>())).Returns(true);

        // Act
        var result = _libraryManagerMock.Object.TrySaveLibrary(mockLibraryKeeper.Object, "path/to/folder");

        // Assert
        Xunit.Assert.True(result);
    }

    /// <summary>
    /// Tests if a book is added correctly to the library.
    /// </summary>
    [Fact]
    public void AddBook_ShouldAddBookToLibrary()
    {
        // Arrange
        var book = new Book { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100, PublishDate = 2020 };

        // Act
        _bookManagerMock.Object.AddBook(book);

        // Assert
        Xunit.Assert.Single(_bookManagerMock.Object.Library.BookList);
        Xunit.Assert.Contains(book, _bookManagerMock.Object.Library.BookList);
    }

    /// <summary>
    /// Tests if a book is removed correctly from the library.
    /// </summary>
    [Fact]
    public void RemoveBook_ShouldRemoveBookFromLibrary()
    {
        // Arrange
        var book = new Book { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100, PublishDate = 2020 };
        _bookManagerMock.Object.AddBook(book);

        // Act
        var result = _bookManagerMock.Object.TryRemoveBook(book);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.Empty(_bookManagerMock.Object.Library.BookList);
    }

    /// <summary>
    /// Tests if the library is sorted correctly by author and title.
    /// </summary>
    [Fact]
    public void SortLibrary_ShouldSortBooksByAuthorAndTitle()
    {
        // Arrange
        var book1 = new Book { Id = 1, Author = "AuthorB", Title = "Title2", TotalPages = 100, PublishDate = 2020 };
        var book2 = new Book { Id = 2, Author = "AuthorA", Title = "Title1", TotalPages = 200, PublishDate = 2021 };
        _bookManagerMock.Object.AddBook(book1);
        _bookManagerMock.Object.AddBook(book2);
        // Act
        _bookManagerMock.Object.SortBooks();

        // Assert
        Xunit.Assert.Equal(book2, _bookManagerMock.Object.Library.BookList[0]);
        Xunit.Assert.Equal(book1, _bookManagerMock.Object.Library.BookList[1]);
    }

    /// <summary>
    /// Tests if books are found correctly by a specific book element.
    /// </summary>
    [Fact]
    public void FindBooksByBookElement_ShouldReturnBooksByAuthor()
    {
        // Arrange
        var book = new Book { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100, PublishDate = 2020 };
        _bookManagerMock.Object.AddBook(book);

        // Act
        var result = _bookManagerMock.Object.FindBooksByKind(EBibliographicKindInformation.Author, "Author1");

        // Assert
        Xunit.Assert.Single(result);
        Xunit.Assert.Contains(book, result);
    }
}
