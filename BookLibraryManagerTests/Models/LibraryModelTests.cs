using Xunit;

namespace BookLibraryManager.Models.Tests;

public class LibraryModelTests
{
    #region Unit tests for AddBook()
    [Fact()]
    public void AddBook_OneBook_LibraryShouldIncreasedByOneBook()
    {
        //Arrange
        var library = new LibraryModel() { Id = 1, BookList = [] };
        var expectedQuantityBooks = 0;
        
        //Act
        library.AddBook(Book_AA);
        expectedQuantityBooks += 1;

        //Assert
        var currentQuantityBooks = library.AmountBooks;
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
    public void SortLibrary_ReverseSortedOrder_FirstBookShouldBecameLastBook()
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






    #region private methods
    private Book Book_AA => new() { Id = 1, Author = "a", Title = "a", PageNumber = 1 };
    private Book Book_AB => new() { Id = 2, Author = "a", Title = "b", PageNumber = 1 };
    private Book Book_BA => new() { Id = 3, Author = "b", Title = "a", PageNumber = 1 };
    #endregion

}