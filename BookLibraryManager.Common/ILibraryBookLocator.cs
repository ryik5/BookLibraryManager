namespace BookLibraryManager.Common;

/// <summary>
/// Interface for searching books in the library.
/// </summary>
/// <author>YR 2025-01-22</author>
public interface ILibraryBookLocator
{
    /// <summary>
    /// Finds books by a specific element of the book.
    /// </summary>
    /// <param name="bookElement">The element of the book to search by.</param>
    /// <param name="partOfElement">The value to search for within the specified element.</param>
    /// <returns>A list of books that match the search criteria.</returns>
    List<Book> FindBooksByBookElement(BookElementsEnum bookElement, object partOfElement);
}
