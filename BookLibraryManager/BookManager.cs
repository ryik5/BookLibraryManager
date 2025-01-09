using BookLibraryManager.Models;

namespace BookLibraryManager;

public class BookManager : IBookLibraryManageable
{
    public ILibrary NewLibrary(int idLibrary)
    {
        return LibraryModel.GetNewLibrary(idLibrary);
    }

    public bool LoadLibrary(IBookListLoadable loader, string pathToFile, out ILibrary library)
    {
        library = loader.LoadLibrary(pathToFile);

        return library is ILibrary;
    }

    public void AddBook(ILibrary library, Book book)
    {
        library.AddBook(book);
    }

    public bool RemoveBook(ILibrary library, Book book)
    {
        return library.RemoveBook(book);
    }

    public void SortLibrary(ILibrary library)
    {
        library.SortLibrary();
    }

    public bool SaveLibrary(IBookListSaveable keeper, string pathToFolder, ILibrary library)
    {
        return keeper.SaveLibrary(library, pathToFolder);
    }

    public List<Book> FindBooksByTitle(ILibrary library, string partOfTitle)
    {
        return library.BookList.FindAll(b => b.Title.Contains(partOfTitle, StringComparison.OrdinalIgnoreCase));
    }
}
