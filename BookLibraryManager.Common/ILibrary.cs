using System.Collections.ObjectModel;

namespace BookLibraryManager.Common;

/// <summary>
/// Represents a library interface that provides functionalities to add, remove, sort, and display books.
/// </summary>
/// <author>YR 2025-01-09</author>
public interface ILibrary
{
    /// <summary>
    /// Gets or sets the unique identifier for the library.
    /// </summary>
    int Id
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the collection of books in the library.
    /// </summary>
    ObservableCollection<Book> BookList
    {
        get; set;
    }

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
    bool RemoveBook(Book book);

    /// <summary>
    /// Sorts books in the library.
    /// </summary>
    void SortLibrary();

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
    List<Book> FindBooksByBookElement(BookElementsEnum bookElement, object partOfElement);

    /// <summary>
    /// Gets the total number of books in the library.
    /// </summary>
    int NumberOfBooks
    {
        get;
    }

}
