using BookLibraryManager.Models;
using Xunit;

namespace BookLibraryManagerTests;

public class LibraryModelTests
{
    #region Unit tests for AddBook()
    [Fact()]
    public void AddBook_OneBook_BookQuantityShouldBeIncreasedByOne()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        var expectedQuantityBooks = 0;

        //Act
        library.AddBook(Book_AA);
        expectedQuantityBooks += 1; // 1 book added

        //Assert
        var currentQuantityBooks = library.NumberOfBooks;
        Xunit.Assert.Equal(expectedQuantityBooks, currentQuantityBooks);
    }

    [Fact()]
    public void AddBook_OneBook_LastAddedBookShouldBeLastOne()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        library.AddBook(Book_AA);

        //Act
        var firstBook = new Book() { Id = Random.Shared.Next(), Author = "a", Title = "a", PageNumber = 1 };
        library.AddBook(firstBook);
        var lastBook = new Book() { Id = Random.Shared.Next(), Author = "b", Title = "b", PageNumber = 1 };
        var idExpectedBook = lastBook.Id;
        library.AddBook(lastBook);

        //Assert
        var lastAddedBook = library.BookList.Last();
        var idLastAddedBook = lastAddedBook.Id;

        Xunit.Assert.Equal(idExpectedBook, idLastAddedBook);
    }
    #endregion


    #region Unit tests for RemoveBook()
    [Fact()]
    public void RemoveBook_OneExistedBook_BookQuantityShouldBeDecreasedByOneBook()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        var addedBook = Book_AA;
        library.AddBook(addedBook);
        var expectedQuantityBooks = library.NumberOfBooks; // 1 book in total

        //Act
        library.RemoveBook(addedBook);
        expectedQuantityBooks -= 1; // 1 existed book removed // 0 book in total

        //Assert
        var currentQuantityBooks = library.NumberOfBooks;
        Xunit.Assert.Equal(expectedQuantityBooks, currentQuantityBooks);
    }

    [Fact()]
    public void RemoveBook_OneNotExistedBook_BookQuantityShouldNotBeChanged()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        var addedBook = Book_AA;
        library.AddBook(addedBook);
        var expectedQuantityBooks = library.NumberOfBooks; // 1 book in total

        //Act
        var unexistedBook = Book_AB;
        library.RemoveBook(unexistedBook);  // try to remove unexisted book // 1 book should be remain

        //Assert
        var currentQuantityBooks = library.NumberOfBooks;
        Xunit.Assert.Equal(expectedQuantityBooks, currentQuantityBooks);
    }
    #endregion


    #region Unit tests for SortLibrary()
    [Fact()]
    public void SortLibrary_SortedOrder_FirstBookShouldBeSame()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        library.AddBook(Book_AA);
        library.AddBook(Book_AB);

        //Act
        library.SortLibrary();

        //Assert
        var expectedBook = library.BookList.First();
        Xunit.Assert.Equal(expectedBook.ToString(), Book_AA.ToString());
    }

    [Fact()]
    public void SortLibrary_ReverseSortedOrder_FirstBookShouldBecomeLastBook()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        library.AddBook(Book_AB);
        library.AddBook(Book_AA);

        //Act
        library.SortLibrary();

        //Assert
        var expectedBook = library.BookList.Last();
        Xunit.Assert.Equal(expectedBook.ToString(), Book_AB.ToString());
    }

    [Fact()]
    public void SortLibrary_SortedOrder_LastBookShouldBeSame()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        library.AddBook(Book_AA);
        library.AddBook(Book_AB);

        //Act
        library.SortLibrary();

        //Assert
        var expectedBook = library.BookList.Last();
        Xunit.Assert.Equal(expectedBook.ToString(), Book_AB.ToString());
    }

    [Fact()]
    public void SortLibrary_ReverseSortedOrder_LastBookShouldBecomeFirst()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        library.AddBook(Book_AB);
        library.AddBook(Book_AA);

        //Act
        library.SortLibrary();

        //Assert
        var expectedBook = library.BookList.First();
        Xunit.Assert.Equal(expectedBook.ToString(), Book_AA.ToString());
    }

    [Fact()]
    public void SortLibrary_SortedOrder_ResultShouldBeEqual()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        library.AddBook(Book_AA);
        library.AddBook(Book_AB);
        library.AddBook(Book_BA);
        var cloneLibrary = library.Clone();

        //Act
        library.SortLibrary();

        //Assert
        Xunit.Assert.Equal(library.ToString(), cloneLibrary.ToString());
    }

    [Fact()]
    public void SortLibrary_ReverseSortedOrder_ResultShouldBeNotEqual()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        library.AddBook(Book_BA);
        library.AddBook(Book_AB);
        library.AddBook(Book_AA);
        var cloneLibrary = library.Clone();

        //Act
        library.SortLibrary();

        //Assert
        Xunit.Assert.NotEqual(library.ToString(), cloneLibrary.ToString());
    }
    #endregion


    [Fact()]
    public void GetNewLibrary_ShouldReturnNewEmptyLibrary()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };

        //Act
        var newLibrary = LibraryModel.GetNewLibrary(1);

        //Assert
        Xunit.Assert.NotNull(newLibrary);
        Xunit.Assert.NotNull(newLibrary.BookList);
        Xunit.Assert.Empty(newLibrary.BookList);
    }

    [Fact()]
    public void GetFirstBooks_AddedTwoBooks_ShouldReturnFirstAddedBook()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };

        //Act
        var expectedBook = new Book { Id = 1, Author = "author", Title = "title", PageNumber = 1 };
        var unexpectedBook = new Book { Id = 2, Author = "unexpected author", Title = "unexpected title", PageNumber = 1 };
        library.AddBook(expectedBook);
        library.AddBook(unexpectedBook);
        var expectedList = library.GetFirstBooks(1);

        //Assert
        Xunit.Assert.Equal(expectedBook, expectedList.First());
    }

    [Fact()]
    public void GetFirstBooks_AddedTwoBooks_ShouldReturnOneBook()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };

        //Act
        var expectedBook = new Book { Id = 1, Author = "author", Title = "title", PageNumber = 1 };
        var unexpectedBook = new Book { Id = 2, Author = "unexpected author", Title = "unexpected title", PageNumber = 1 };
        library.AddBook(expectedBook);
        library.AddBook(unexpectedBook);

        //Assert
        Xunit.Assert.Single(library.GetFirstBooks(1));
    }



    #region private methods
    private Book Book_AA => new() { Id = 1, Author = "a", Title = "a", PageNumber = 1 };
    private Book Book_AB => new() { Id = 2, Author = "a", Title = "b", PageNumber = 1 };
    private Book Book_BA => new() { Id = 3, Author = "b", Title = "a", PageNumber = 1 };
    #endregion
}