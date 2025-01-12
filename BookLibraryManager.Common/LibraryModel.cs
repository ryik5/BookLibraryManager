namespace BookLibraryManager.Common;

/// <summary>
/// Represents a model for managing a library of books.
/// </summary>
/// <author>YR 2025-01-09</author>
public class LibraryModel : LibraryAbstract, ILibrary
{
    /// <summary>
    /// Adds a book to the library.
    /// </summary>
    /// <param name="book">The book to add.</param>
    public void AddBook(Book book)
    {
        BookList.Add(book);
    }

    /// <summary>
    /// Removes a book from the library.
    /// </summary>
    /// <param name="book">The book to remove.</param>
    /// <returns>True if the book was successfully removed; otherwise, false.</returns>
    public bool RemoveBook(Book book)
    {
        var searchBook = BookList.Find(b => b.Id == book.Id);
        return BookList.Remove(searchBook);
    }

    /// <summary>
    /// Sorts the library by author and then by title.
    /// </summary>
    public void SortLibrary()
    {
        BookList = BookList.OrderBy(b => b.Author).ThenBy(b => b.Title).ToList();
    }

    /// <summary>
    /// Creates a new library with the specified ID.
    /// </summary>
    /// <param name="idLibrary">The ID of the new library.</param>
    /// <returns>A new instance of LibraryModel.</returns>
    public static LibraryModel GetNewLibrary(int idLibrary)
    {
        return new LibraryModel { Id = idLibrary, BookList = new List<Book>() };
    }

    /// <summary>
    /// Retrieves a list of the first specified number of books from the library.
    /// </summary>
    /// <param name="amountFirstBooks">The number of books to retrieve.</param>
    /// <returns>A list of the first books.</returns>
    public List<Book> GetFirstBooks(int amountFirstBooks) => BookList.Take(amountFirstBooks).ToList();

    /// <summary>
    /// Retrieves all books in the library.
    /// </summary>
    /// <returns>A list of all books.</returns>
    public List<Book> GetAllBooks() => BookList;

    /// <summary>
    /// Gets the total number of books in the library.
    /// </summary>
    public int NumberOfBooks => BookList.Count;

    /// <summary>
    /// Displays the first specified number of books from the library as a string.
    /// </summary>
    /// <param name="amountFirstBooks">The number of books to display.</param>
    /// <returns>A string representation of the first books.</returns>
    public string ShowFistBooks(int amountFirstBooks)
    {
        var list = GetFirstBooks(amountFirstBooks).Select(bookSelector);
        return JoinStrings(list);
    }

    /// <summary>
    /// Displays the last specified number of books from the library as a string.
    /// </summary>
    /// <param name="amountLastBooks">The number of books to display.</param>
    /// <returns>A string representation of the last books.</returns>
    public string ShowLastBooks(int amountLastBooks)
    {
        var availableAmountBooks = NumberOfBooks < amountLastBooks ? NumberOfBooks : amountLastBooks;
        var list = BookList.Take(availableAmountBooks).Reverse().Select(bookSelector);

        return JoinStrings(list);
    }

    /// <summary>
    /// Creates a full clone of the current library instance.
    /// </summary>
    /// <returns>A new instance of LibraryModel that is a full copy of the current instance.</returns>
    public ILibrary Clone()
    {
        return new LibraryModel { Id = Id, BookList = new List<Book>(BookList.Select(b => new Book { Id = b.Id, Author = b.Author, PageNumber = b.PageNumber, Title = b.Title })) };
    }

    /// <summary>
    /// Returns a string representation of the full library.
    /// </summary>
    /// <returns>A string that represents the library.</returns>
    public override string ToString()
    {
        return $"{Id}-{string.Join(",", BookList.Select(b => b))}";
    }

    #region private methods
    /// <summary>
    /// A function to select a string representation of a book.
    /// </summary>
    private Func<Book, string> bookSelector = b => $"{b.Id}. Author:{b.Author} - Title:{b.Title}";

    /// <summary>
    /// Joins a list of strings into a single string with each element separated by a comma and newline.
    /// </summary>
    /// <param name="list">The list of strings to join.</param>
    /// <returns>A single string with each element separated by a comma and newline.</returns>
    private string JoinStrings(IEnumerable<string> list) => string.Join(",\n", list);
    #endregion
}
