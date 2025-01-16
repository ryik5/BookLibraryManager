namespace BookLibraryManager.Common;

/// <summary>
/// Interface of the Book Library Management
/// </summary>
/// <author>YR 2025-01-09</author>
public interface IBookLibraryManageable
{
    /// <summary>
    /// Creates a new instance of the library with the specified identifier.
    /// </summary>
    /// <param name="idLibrary">The unique identifier for the new library.</param>
    /// <returns>A new instance of <see cref="ILibrary"/>.</returns>
    ILibrary CreateNewLibrary(int idLibrary);

    /// <summary>
    /// Loads a library from the specified path.
    /// </summary>
    /// <param name="loader">The loader used to load the library.</param>
    /// <param name="pathToLibrary">The path to the library file.</param>
    /// <param name="library">When this method returns, contains the loaded library if the load was successful; otherwise, null.</param>
    /// <returns>true if the library was successfully loaded; otherwise, false.</returns>
    bool LoadLibrary(IBookListLoadable loader, string pathToLibrary, out ILibrary library);

    /// <summary>
    /// Adds a book to the specified library.
    /// </summary>
    /// <param name="library">The library to which the book will be added.</param>
    /// <param name="book">The book to add.</param>
    void AddBook(ILibrary library, Book book);

    /// <summary>
    /// Removes a book from the specified library.
    /// </summary>
    /// <param name="library">The library from which the book will be removed.</param>
    /// <param name="book">The book to remove.</param>
    /// <returns>true if the book was successfully removed; otherwise, false.</returns>
    bool RemoveBook(ILibrary library, Book book);

    /// <summary>
    /// Sorts the books in the specified library.
    /// </summary>
    /// <param name="library">The library to sort.</param>
    void SortLibrary(ILibrary library);

    /// <summary>
    /// Finds books in the specified library that contain the specified title part.
    /// </summary>
    /// <param name="library">The library to search.</param>
    /// <param name="partOfTitle">The part of the title to search for.</param>
    /// <returns>A list of books that contains the specified title part.</returns>
    List<Book> FindBooksByTitle(ILibrary library, string partOfTitle);

    /// <summary>
    /// Saves the specified library to the specified path.
    /// </summary>
    /// <param name="keeper">The keeper used to save the library.</param>
    /// <param name="pathToLibrary">The path to the library file.</param>
    /// <param name="library">The library to save.</param>
    /// <returns>true if the library was successfully saved; otherwise, false.</returns>
    bool SaveLibrary(IBookListSaveable keeper, string pathToLibrary, ILibrary library);
}
