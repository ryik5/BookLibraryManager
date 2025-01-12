namespace BookLibraryManager.Common;

/// <summary>
/// Interface for displaying books in a library.
/// </summary>
/// <author>YR 2025-01-09</author>
public interface ILibraryShower
{
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
    /// Gets the total number of books in the library.
    /// </summary>
    int NumberOfBooks { get; }

    /// <summary>
    /// Displays the first specified number of books from the library as a string
    /// </summary>
    /// <param name="amountFirstBooks">The number of books for showing.</param>
    string ShowFistBooks(int amountFirstBooks);

    /// <summary>
    /// Displays the last specified number of books from the library as a string.
    /// </summary>
    /// <param name="amountLastBooks">The number of books for showing.</param>
    string ShowLastBooks(int amountLastBooks);
}
