namespace BookLibraryManager.Common;

/// <summary>
/// Provides an interface for removing a book from the library.
/// </summary>
/// <author>YR 2025-01-09</author>
public interface ILibraryBookRemover
{
    /// <summary>
    /// Removes the specified book from the library.
    /// </summary>
    /// <param name="book">The book to remove.</param>
    /// <returns>true if the book was successfully removed; otherwise, false.</returns>
    bool RemoveBook(Book book);
}
