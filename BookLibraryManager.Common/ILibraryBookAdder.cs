namespace BookLibraryManager.Common;

/// <summary>
/// Defines the contract for adding a book to the library.
/// </summary>
/// <author>YR 2025-01-09</author>
public interface ILibraryBookAdder
{
    /// <summary>
    /// Adds a book to the library.
    /// </summary>
    /// <param name="book">The book to add.</param>
    void AddBook(Book book);
}
