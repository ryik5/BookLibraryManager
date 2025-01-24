using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

/// <author>YR 2025-01-09</author>
public class LibraryBookManagerModelTests
{
    /// <summary>
    /// Tests if a new library is initialized correctly.
    /// </summary>
    [Fact]
    public void CreateNewLibrary_ShouldInitializeLibrary()
    {
        // Arrange
        var library = new LibraryBookManagerModel();
        int libraryId = 1;

        // Act
        library.CreateNewLibrary(libraryId);

        // Assert
        Xunit.Assert.Equal(libraryId, library.Id);
        Xunit.Assert.Empty(library.BookList);
    }

    /// <summary>
    /// Tests if a library is loaded correctly from a file.
    /// </summary>
    [Fact]
    public void LoadLibrary_ShouldLoadLibraryFromFile()
    {
        // Arrange
        var mockLibraryLoader = new Mock<ILibraryLoader>();
        var library = new LibraryBookManagerModel();
        ILibrary loadedLibrary = new LibraryBookManagerModel
        {
            Id = 1,
            BookList =
            [
                new() { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100, PublishDate = 2020 }
            ]
        };
        mockLibraryLoader.Setup(loader => loader.LoadLibrary(It.IsAny<string>(), out loadedLibrary)).Returns(true);

        // Act
        var result = library.LoadLibrary(mockLibraryLoader.Object, "path/to/file");

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
        var library = new LibraryBookManagerModel();
        mockLibraryKeeper.Setup(keeper => keeper.SaveLibrary(library, It.IsAny<string>())).Returns(true);

        // Act
        var result = library.SaveLibrary(mockLibraryKeeper.Object, "path/to/folder");

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
        var library = new LibraryBookManagerModel();
        library.CreateNewLibrary(1);
        var book = new Book { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100, PublishDate = 2020 };

        // Act
        library.AddBook(book);

        // Assert
        Xunit.Assert.Single(library.BookList);
        Xunit.Assert.Contains(book, library.BookList);
    }

    /// <summary>
    /// Tests if a book is removed correctly from the library.
    /// </summary>
    [Fact]
    public void RemoveBook_ShouldRemoveBookFromLibrary()
    {
        // Arrange
        var library = new LibraryBookManagerModel();
        library.CreateNewLibrary(1);
        var book = new Book { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100, PublishDate = 2020 };
        library.AddBook(book);

        // Act
        var result = library.RemoveBook(book);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.Empty(library.BookList);
    }

    /// <summary>
    /// Tests if the library is sorted correctly by author and title.
    /// </summary>
    [Fact]
    public void SortLibrary_ShouldSortBooksByAuthorAndTitle()
    {
        // Arrange
        var library = new LibraryBookManagerModel();
        library.CreateNewLibrary(1);
        var book1 = new Book { Id = 1, Author = "AuthorB", Title = "Title2", TotalPages = 100, PublishDate = 2020 };
        var book2 = new Book { Id = 2, Author = "AuthorA", Title = "Title1", TotalPages = 200, PublishDate = 2021 };
        library.AddBook(book1);
        library.AddBook(book2);

        // Act
        library.SortLibrary();

        // Assert
        Xunit.Assert.Equal(book2, library.BookList[0]);
        Xunit.Assert.Equal(book1, library.BookList[1]);
    }

    /// <summary>
    /// Tests if books are found correctly by a specific book element.
    /// </summary>
    [Fact]
    public void FindBooksByBookElement_ShouldReturnBooksByAuthor()
    {
        // Arrange
        var library = new LibraryBookManagerModel();
        library.CreateNewLibrary(1);
        var book = new Book { Id = 1, Author = "Author1", Title = "Title1", TotalPages = 100, PublishDate = 2020 };
        library.AddBook(book);

        // Act
        var result = library.FindBooksByBookElement(BookElementsEnum.Author, "Author1");

        // Assert
        Xunit.Assert.Single(result);
        Xunit.Assert.Contains(book, result);
    }
}
