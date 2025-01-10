using BookLibraryManager.Common;
using Moq;
using Xunit;

namespace BookLibraryManager.Tests;

public class BookManagerTests
{
    [Fact()]
    public void NewLibrary_SouldBeNotNull()
    {
        // Arrange
        var bookManager = new BookManager();
        var libraryId = 1;

        // Act
        var library = bookManager.NewLibrary(libraryId);

        // Assert
        Xunit.Assert.NotNull(library.BookList);
        Xunit.Assert.Equal(library.Id, libraryId);
    }

    [Fact]
    public void AddBook_ShouldAddBookToLibrary()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var book = new Book { Id = 1, Author = "Author", Title = "Title", PageNumber = 1 };

        var bookManager = new BookManager();

        // Act
        bookManager.AddBook(mockLibrary.Object, book);

        // Assert
        mockLibrary.Verify(l => l.AddBook(book), Times.Once);
    }

    [Fact()]
    public void AddBook_OneBook_BookQuantityShouldBeIncreasedByOne()
    {
        // Arrange
        var bookManager = new BookManager();
        var library = bookManager.NewLibrary(1);
        var book = new Book() { Id = 1, Title = "Book 1", Author = "Author 1", PageNumber = 1 };

        // Act
        bookManager.AddBook(library, book);

        // Assert
        Xunit.Assert.Equal(1, library.NumberOfBooks);
    }

    [Fact()]
    public void AddBook_OneBook_LastAddedBookShouldBeLastOne()
    {
        //Arrange
        var bookManager = new BookManager();
        var library = bookManager.NewLibrary(1);

        //Act
        var firstBook = new Book() { Id = 1, Author = "a", Title = "a", PageNumber = 1 };
        bookManager.AddBook(library, firstBook);

        var lastBook = new Book() { Id = 2, Author = "b", Title = "b", PageNumber = 1 };
        var idExpectedBook = lastBook.Id;
        bookManager.AddBook(library, lastBook);

        //Assert
        var lastAddedBook = library.BookList.Last();
        var idLastAddedBook = lastAddedBook.Id;

        Xunit.Assert.Equal(2, library.NumberOfBooks);
        Xunit.Assert.Equal(idExpectedBook, idLastAddedBook);
    }

    [Fact]
    public void RemoveBook_ShouldRemoveBookFromLibrary()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var bookManager = new BookManager();
        var book = new Book { Id = 1, Author = "Author", Title = "Title", PageNumber = 1 };
        mockLibrary.Setup(l => l.RemoveBook(book)).Returns(true);

        // Act
        var result = bookManager.RemoveBook(mockLibrary.Object, book);

        // Assert
        Xunit.Assert.True(result);
        mockLibrary.Verify(l => l.RemoveBook(book), Times.Once);
    }

    [Fact()]
    public void RemoveBook_RemoveExistedBook_ShouldBeTrue()
    {
        // Arrange
        var bookManager = new BookManager();

        // Act
        var library = bookManager.NewLibrary(1);
        var book = new Book() { Id = 1, Title = "Book 1", Author = "Author 1", PageNumber = 1 };
        library.AddBook(book);
        var result = bookManager.RemoveBook(library, book);

        // Assert
        Xunit.Assert.Equal(0, library.NumberOfBooks);
        Xunit.Assert.True(result);
    }

    [Fact()]
    public void RemoveBook_RemoveUnexistedBook_ShouldBeFalse()
    {
        // Arrange
        var bookManager = new BookManager();

        // Act
        var library = bookManager.NewLibrary(1);
        var bookExisted = new Book() { Id = 1, Title = "Book 1", Author = "Author 1", PageNumber = 1 };
        var bookUnexisted = new Book() { Id = 2, Title = "Book 2", Author = "Author 2", PageNumber = 1 };
        library.AddBook(bookExisted);
        var result = bookManager.RemoveBook(library, bookUnexisted);

        // Assert
        Xunit.Assert.Equal(1, library.NumberOfBooks);
        Xunit.Assert.False(result);
    }

    [Fact]
    public void SortLibrary_ShouldSortLibrary()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var bookManager = new BookManager();

        // Act
        bookManager.SortLibrary(mockLibrary.Object);

        // Assert
        mockLibrary.Verify(l => l.SortLibrary(), Times.Once);
    }

    [Fact()]
    public void SortLibrary_SortedOrder_FirstInputedBookShouldBeFirstOne()
    {
        // Arrange
        var bookManager = new BookManager();
        var library = bookManager.NewLibrary(1);
        var firstBook = new Book() { Id = 1, Author = "a", Title = "a", PageNumber = 1 };
        bookManager.AddBook(library, firstBook);
        var secondBook = new Book() { Id = 2, Author = "b", Title = "a", PageNumber = 1 };
        bookManager.AddBook(library, secondBook);
        var thirdBook = new Book() { Id = 3, Author = "b", Title = "c", PageNumber = 1 };
        bookManager.AddBook(library, thirdBook);

        // Act
        bookManager.SortLibrary(library);

        // Assert
        var expectedBook = library.BookList.First();
        Xunit.Assert.Equal(firstBook, expectedBook);
    }

    [Fact()]
    public void SortLibrary_ReverseSortedOrder_FirstInputedBookShouldBeLastOne()
    {
        // Arrange
        var bookManager = new BookManager();
        var library = bookManager.NewLibrary(1);
        var firstBook = new Book() { Id = 1, Author = "b", Title = "c", PageNumber = 1 };
        bookManager.AddBook(library, firstBook);
        var secondBook = new Book() { Id = 2, Author = "b", Title = "a", PageNumber = 1 };
        bookManager.AddBook(library, secondBook);
        var thirdBook = new Book() { Id = 3, Author = "a", Title = "a", PageNumber = 1 };
        bookManager.AddBook(library, thirdBook);

        // Act
        bookManager.SortLibrary(library);

        // Assert
        var expectedBook = library.BookList.Last();
        Xunit.Assert.Equal(firstBook, expectedBook);
    }

    [Fact]
    public void FindBooksByTitle_ShouldReturnBooksContainingTitle()
    {
        // Arrange
        var mockLibrary = new Mock<ILibrary>();
        var bookManager = new BookManager();
        var books = new List<Book>
        {
            new() { Id = 1, Author = "Author1", Title = "Title1", PageNumber = 1 },
            new() { Id = 2, Author = "Author2", Title = "AnotherTitle", PageNumber = 1 },
            new() { Id = 3, Author = "Author3", Title = "AnotherBook", PageNumber = 1 }

        };
        mockLibrary.Setup(l => l.BookList).Returns(books);
        var expectedQuantity = 2;

        // Act
        var result = bookManager.FindBooksByTitle(mockLibrary.Object, "Title");

        // Assert
        Xunit.Assert.Equal(expectedQuantity, result.Count);
    }

    [Fact()]
    public void FindBooksByTitle_EveryTitleHasFullyUniqueCollectionChars_ShouldReturnOneBook()
    {
        // Arrange
        var bookManager = new BookManager();
        var library = bookManager.NewLibrary(1);
        var firstBook = new Book() { Id = 1, Author = "b", Title = "qwertyu", PageNumber = 1 };
        bookManager.AddBook(library, firstBook);
        var secondBook = new Book() { Id = 2, Author = "D", Title = "iopasdf", PageNumber = 1 };
        bookManager.AddBook(library, secondBook);
        var thirdBook = new Book() { Id = 3, Author = "F", Title = "ghklzxc", PageNumber = 1 };
        bookManager.AddBook(library, thirdBook);
        var fothExpectedBook = new Book() { Id = 4, Author = "a", Title = "vbnm", PageNumber = 1 };
        bookManager.AddBook(library, fothExpectedBook);

        // Act
        var listExpectedBooks = bookManager.FindBooksByTitle(library, "a");

        // Assert
        var expectedBook = listExpectedBooks.First();
        var expectedQuantity = 1;
        Xunit.Assert.Equal(listExpectedBooks.Count, expectedQuantity);
        Xunit.Assert.Equal(expectedBook, secondBook);
    }

    [Fact()]
    public void FindBooksByTitle_TitlesContainSearchStringBothCase_ShouldReturnTwoBooks()
    {
        // Arrange
        var bookManager = new BookManager();
        var library = bookManager.NewLibrary(1);
        var firstBook = new Book() { Id = 1, Author = "b", Title = "qwertyu", PageNumber = 1 };
        bookManager.AddBook(library, firstBook);
        var secondBook = new Book() { Id = 2, Author = "D", Title = "iopasdf", PageNumber = 1 };
        bookManager.AddBook(library, secondBook);
        var thirdBook = new Book() { Id = 3, Author = "F", Title = "ghjklzxc", PageNumber = 1 };
        bookManager.AddBook(library, thirdBook);
        var fothExpectedBook = new Book() { Id = 4, Author = "a", Title = "vbnm", PageNumber = 1 };
        bookManager.AddBook(library, fothExpectedBook);
        var firstBookUpperCase = new Book() { Id = 6, Author = "b", Title = "QWERTYU", PageNumber = 1 };
        bookManager.AddBook(library, firstBookUpperCase);
        var secondBookUpperCase = new Book() { Id = 6, Author = "D", Title = "IOPASDF", PageNumber = 1 };
        bookManager.AddBook(library, secondBookUpperCase);
        var thirdBookUpperCase = new Book() { Id = 7, Author = "F", Title = "GHJKLZXC", PageNumber = 1 };
        bookManager.AddBook(library, thirdBookUpperCase);
        var fothExpectedBookUpperCase = new Book() { Id = 8, Author = "a", Title = "VBNM", PageNumber = 1 };
        bookManager.AddBook(library, fothExpectedBookUpperCase);
        // Act
        var listExpectedBooks = bookManager.FindBooksByTitle(library, "a");

        // Assert
        var expectedQuantity = 2;
        Xunit.Assert.Equal(listExpectedBooks.Count, expectedQuantity);
    }

    [Fact()]
    public void FindBooksByTitle_TitlesContainSearchStringOpositCase_ShouldReturnOneBook()
    {
        // Arrange
        var bookManager = new BookManager();
        var library = bookManager.NewLibrary(1);
        var firstBookUpperCase = new Book() { Id = 6, Author = "b", Title = "QWERTYU", PageNumber = 1 };
        bookManager.AddBook(library, firstBookUpperCase);
        var secondBookUpperCase = new Book() { Id = 6, Author = "D", Title = "IOPASDF", PageNumber = 1 };
        bookManager.AddBook(library, secondBookUpperCase);
        var thirdBookUpperCase = new Book() { Id = 7, Author = "F", Title = "GHJKLZXC", PageNumber = 1 };
        bookManager.AddBook(library, thirdBookUpperCase);
        var fothExpectedBookUpperCase = new Book() { Id = 8, Author = "a", Title = "VBNM", PageNumber = 1 };
        bookManager.AddBook(library, fothExpectedBookUpperCase);
        // Act
        var listExpectedBooks = bookManager.FindBooksByTitle(library, "a");

        // Assert
        var expectedQuantity = 1;
        Xunit.Assert.Equal(listExpectedBooks.Count, expectedQuantity);
    }

    [Fact()]
    public void FindBooksByTitle_NoTitleContainSearchString_ShouldReturnNothing()
    {
        // Arrange
        var bookManager = new BookManager();
        var library = bookManager.NewLibrary(1);
        var firstBook = new Book() { Id = 1, Author = "b", Title = "qwertyu", PageNumber = 1 };
        bookManager.AddBook(library, firstBook);
        var secondBook = new Book() { Id = 2, Author = "D", Title = "iopasdf", PageNumber = 1 };
        bookManager.AddBook(library, secondBook);
        var thirdBook = new Book() { Id = 3, Author = "F", Title = "ghjklzxc", PageNumber = 1 };
        bookManager.AddBook(library, thirdBook);
        var fothExpectedBook = new Book() { Id = 4, Author = "a", Title = "vbnm", PageNumber = 1 };
        bookManager.AddBook(library, fothExpectedBook);
        var firstBookUpperCase = new Book() { Id = 6, Author = "b", Title = "QWERTYU", PageNumber = 1 };
        bookManager.AddBook(library, firstBookUpperCase);
        var secondBookUpperCase = new Book() { Id = 6, Author = "D", Title = "IOPASDF", PageNumber = 1 };
        bookManager.AddBook(library, secondBookUpperCase);
        var thirdBookUpperCase = new Book() { Id = 7, Author = "F", Title = "GHJKLZXC", PageNumber = 1 };
        bookManager.AddBook(library, thirdBookUpperCase);
        var fothExpectedBookUpperCase = new Book() { Id = 8, Author = "a", Title = "VBNM", PageNumber = 1 };
        bookManager.AddBook(library, fothExpectedBookUpperCase);

        // Act
        var listExpectedBooks = bookManager.FindBooksByTitle(library, "1");

        // Assert
        var expectedQuantity = 0;
        Xunit.Assert.Equal(listExpectedBooks.Count, expectedQuantity);
    }

    [Fact()]
    public void LoadLibraryTest()
    {
        // Arrange
        var mockLoader = new Mock<IBookListLoadable>();
        var mockLibrary = new Mock<ILibrary>();
        mockLoader.Setup(loader => loader.LoadLibrary(It.IsAny<string>())).Returns(mockLibrary.Object);

        var bookManager = new BookManager();
        var pathToFile = "testPath";
        ILibrary library;

        // Act
        var result = bookManager.LoadLibrary(mockLoader.Object, pathToFile, out library);

        // Assert
        Xunit.Assert.True(result);
        Xunit.Assert.Equal(mockLibrary.Object, library);
    }

    [Fact]
    public void SaveLibrary_ShouldSaveLibraryToFile()
    {
        // Arrange
        var mockKeeper = new Mock<IBookListSaveable>();
        var mockLibrary = new Mock<ILibrary>();
        var bookManager = new BookManager();
        var pathToFolder = "testFolder";
        mockKeeper.Setup(keeper => keeper.SaveLibrary(mockLibrary.Object, pathToFolder)).Returns(true);

        // Act
        var result = bookManager.SaveLibrary(mockKeeper.Object, pathToFolder, mockLibrary.Object);

        // Assert
        Xunit.Assert.True(result);
        mockKeeper.Verify(k => k.SaveLibrary(mockLibrary.Object, pathToFolder), Times.Once);
    }
}
