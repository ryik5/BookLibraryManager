namespace BookLibraryManager.Common;

/// <summary>
/// Represents a Book interface that provides functionalities to add, remove, sort, and display books.
/// </summary>
/// <author>YR 2025-01-09</author>
public interface IBookManageable : ILoadable
{
    /// <summary>
    /// Sorts books in the library.
    /// </summary>
    void SortBooks();

    /// <summary>
    /// Adds a book to the library.
    /// </summary>
    /// <param name="book">The book to add.</param>
    void AddBook(Book book);

    /// <summary>
    /// Removes the specified book from the library.
    /// </summary>
    /// <param name="book">The book to remove.</param>
    /// <returns>true if the book was successfully removed; otherwise, false.</returns>
    bool TryRemoveBook(Book book);

    bool TryLoadBook(IBookLoader bookLoader, string pathToFile);

    bool TrySaveBook(IBookKeeper keeper, Book book, string pathToFolder);

    /// <summary>
    /// Retrieves the first specified number of books from the library.
    /// </summary>
    /// <param name="amountFirstBooks">The number of books to retrieve.</param>
    List<Book> GetFirstBooks(int amountFirstBooks);

    /// <summary>
    /// Retrieves all books in the library.
    /// </summary>
    List<Book> GetAllBooks();

    /// <summary>
    /// Finds books by a specific element of the book.
    /// </summary>
    /// <param name="bookElement">The element of the book to search by.</param>
    /// <param name="partOfElement">The value to search for within the specified element.</param>
    /// <returns>A list of books that match the search criteria.</returns>
    List<Book> FindBooksByKind(EBibliographicKindInformation bookElement, object partOfElement);

    /// <summary>
    /// Gets or sets the library.
    /// </summary>
    ILibrary Library
    {
        get; set;
    }
}
