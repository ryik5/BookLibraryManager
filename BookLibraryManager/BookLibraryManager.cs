using BookLibraryManager.Common;

namespace BookLibraryManager;

/// <summary>
/// Manages the operations related to book libraries, including creating new libraries,
/// loading libraries from the path, adding and removing books, sorting books, saving libraries,
/// and finding books by a part of the title.
/// </summary>
/// <author>YR 2025-01-09</author>
public class BookLibraryManager
{
    /// <summary>
    /// Creates a new library with the specified ID.
    /// </summary>
    /// <param name="idLibrary">The unique identifier for the new library.</param>
    /// <returns>A new instance of <see cref="ILibrary"/>.</returns>
    public ILibrary CreateNewLibrary(int idLibrary)
        => LibraryManagerModel.CreateNewLibrary(idLibrary);

    /// <summary>
    /// Loads a library from the specified file path.
    /// </summary>
    /// <param name="loader">The loader responsible for loading the library.</param>
    /// <param name="pathToFile">The path to the file containing the library data.</param>
    /// <param name="library">The loaded library.</param>
    /// <returns>True if the library was successfully loaded; otherwise, false.</returns>
    public bool LoadLibrary(IBookListLoadable loader, string pathToFile, out ILibrary library)
        => loader.LoadLibrary(pathToFile, out library);

    /// <summary>
    /// Adds a book to the specified library.
    /// </summary>
    /// <param name="library">The library to which the book will be added.</param>
    /// <param name="book">The book to add.</param>
    public void AddBook(ILibrary library, Book book)
        => library.AddBook(book);

    /// <summary>
    /// Removes a book from the specified library.
    /// </summary>
    /// <param name="library">The library from which the book will be removed.</param>
    /// <param name="book">The book to remove.</param>
    /// <returns>True if the book was successfully removed; otherwise, false.</returns>
    public bool RemoveBook(ILibrary library, Book book)
        => library.RemoveBook(book);

    /// <summary>
    /// Sorts the books in the specified library.
    /// </summary>
    /// <param name="library">The library to sort.</param>
    public void SortLibrary(ILibrary library)
        => library.SortLibrary();

    /// <summary>
    /// Saves the specified library to the specified folder.
    /// </summary>
    /// <param name="keeper">The keeper responsible for saving the library.</param>
    /// <param name="pathToFolder">The path to the folder where the library will be saved.</param>
    /// <param name="library">The library to save.</param>
    /// <returns>True if the library was successfully saved; otherwise, false.</returns>
    public bool SaveLibrary(IBookListSaveable keeper, string pathToFolder, ILibrary library)
        => keeper.SaveLibrary(library, pathToFolder);

    /// <summary>
    /// Finds books in the specified library by a part of the book elements.
    /// </summary>
    /// <param name="library">The library to search in.</param>
    /// <param name="bookElement">The element of the book to search by.</param>
    /// <param name="partOfElement">The part of the element to search for.</param>
    /// <returns>A list of books that match the search criteria.</returns>
    public List<Book> FindBooksByBookElement(ILibrary library, BookElementsEnum bookElement, object partOfElement)
        => library.FindBooksByBookElement(library, bookElement, partOfElement);
}
