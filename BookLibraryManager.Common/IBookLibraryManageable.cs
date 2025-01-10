namespace BookLibraryManager.Common;

/// <summary>
/// Interface of the Book Library Management
/// </summary>
public interface IBookLibraryManageable
{
    ILibrary NewLibrary(int idLibrary);

    bool LoadLibrary(IBookListLoadable loader, string pathToLibrary, out ILibrary library);

    void AddBook(ILibrary library, Book book);

    bool RemoveBook(ILibrary library, Book book);

    void SortLibrary(ILibrary library);

    List<Book> FindBooksByTitle(ILibrary library, string partOfTitle);

    bool SaveLibrary(IBookListSaveable keeper, string pathToLibrary, ILibrary library);
}
