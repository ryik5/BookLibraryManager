using Xunit;

namespace BookLibraryManager.Models.Tests;

public class LibraryModelTests
{
    [Fact()]
    public void SortLibrary_ResultShouldNotBeEqualWithOriginalAfterOneReverse()
    {
        //Arrange
        var lib = GetLibraryWithThreeBooks();

        //Act
        var newCloneLibrary = lib.Clone();
        ReverseBookOrder(lib);

        lib.SortLibrary();

        //Assert
        Xunit.Assert.Equal(lib?.ToString(), newCloneLibrary?.ToString());
    }

    [Fact()]
    public void SortLibrary_ResultShouldBeEqualWithOriginalAfterTwoReverse()
    {
        //Arrange
        var lib = GetLibraryWithThreeBooks();

        //Act
        var newCloneLibrary = lib.Clone();
        ReverseBookOrder(lib);
        ReverseBookOrder(lib);

        lib.SortLibrary();

        //Assert
        Xunit.Assert.Equal(lib?.ToString(), newCloneLibrary?.ToString());
    }



    private ILibrary GetLibraryWithThreeBooks()
    {
        var lib = new LibraryModel() { Id = 1, BookList = [] };
        lib.AddBook(new Book { Id = 1, Author = "a", Title = "a", PageNumber = 1 });
        lib.AddBook(new Book { Id = 2, Author = "b", Title = "b", PageNumber = 1 });
        lib.AddBook(new Book { Id = 3, Author = "c", Title = "c", PageNumber = 1 });
        return lib;
    }

    private void ReverseBookOrder(ILibrary library)
    {
        library.BookList.Reverse();
    }

}