﻿using BookLibraryManager.Common;
using Xunit;

namespace BookLibraryManager.Tests;

public class BookManagerTests
{
    [Fact()]
    public void NewLibrary_SouldBeNotNull()
    {
        // Arrange
        var bookManager = new BookManager();

        // Act
        var library = bookManager.NewLibrary(1);

        // Assert
        Xunit.Assert.NotNull(library);
        Xunit.Assert.NotNull(library.BookList);
        Xunit.Assert.Empty(library.BookList);
    }

    [Fact()]
    public void AddBook_OneBook_BookQuantityShouldBeIncreasedByOne()
    {
        // Arrange
        var bookManager = new BookManager();
        var library = bookManager.NewLibrary(1);
        var book = new Book() { Id = 1, Title = "Book 1", Author = "Author 1", PageNumber = 20 };

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

    [Fact()]
    public void RemoveBook_RemoveExistedBook_ShouldBeTrue()
    {
        // Arrange
        var bookManager = new BookManager();

        // Act
        var library = bookManager.NewLibrary(1);
        var book = new Book() { Id = 1, Title = "Book 1", Author = "Author 1", PageNumber = 20 };
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
        var bookExisted = new Book() { Id = 1, Title = "Book 1", Author = "Author 1", PageNumber = 20 };
        var bookUnexisted = new Book() { Id = 2, Title = "Book 2", Author = "Author 2", PageNumber = 20 };
        library.AddBook(bookExisted);
        var result = bookManager.RemoveBook(library, bookUnexisted);

        // Assert
        Xunit.Assert.Equal(1, library.NumberOfBooks);
        Xunit.Assert.False(result);
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

    [Fact()]
    public void FindBooksByTitle_LibraryHasFullyUniqueCollectionCharInTitle_ShouldReturnOneBook()
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
    public void FindBooksByTitle_LibraryHasBooksWithPartlyDuplicatedTitles_ShouldReturnThreeBooks()
    {
        // Arrange
        var bookManager = new BookManager();
        var library = bookManager.NewLibrary(1);
        var firstExpectedBook = new Book() { Id = 1, Author = "b", Title = "cda", PageNumber = 1 };
        bookManager.AddBook(library, firstExpectedBook);
        var secondExpectedBook = new Book() { Id = 2, Author = "D", Title = "asb", PageNumber = 1 };
        bookManager.AddBook(library, secondExpectedBook);
        var thirdBook = new Book() { Id = 3, Author = "F", Title = "ebc", PageNumber = 1 };
        bookManager.AddBook(library, thirdBook);
        var fothExpectedBook = new Book() { Id = 4, Author = "D", Title = "Ags", PageNumber = 1 };
        bookManager.AddBook(library, fothExpectedBook);

        // Act
        var listExpectedBooks = bookManager.FindBooksByTitle(library, "a");
        var expectedNumberOfBooks = 3;

        // Assert
        Xunit.Assert.True(expectedNumberOfBooks == listExpectedBooks.Count);
        Xunit.Assert.Contains(firstExpectedBook, listExpectedBooks);
        Xunit.Assert.Contains(secondExpectedBook, listExpectedBooks);
    }

}
