namespace BookLibraryManager.Common;

public interface ILibrary: ILibraryAddBook, ILibraryRemoveBook, ILibrarySort, ILibraryShower
{
    int Id
    {
        get;
        set;
    }

    List<Book> BookList
    {
        get;
        set;
    }

    ILibrary Clone();
}
