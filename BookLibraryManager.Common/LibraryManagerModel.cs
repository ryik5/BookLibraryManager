using System.Collections.ObjectModel;
using System.Windows;

namespace BookLibraryManager.Common;

/// <summary>
/// Represents a model for managing a library of books.
/// </summary>
/// <author>YR 2025-01-09</author>
public class LibraryManagerModel : LibraryAbstract, ILibrary
{
    /// <summary>
    /// Creates a new library with the specified ID.
    /// </summary>
    /// <param name="idLibrary">The ID of the new library.</param>
    /// <returns>A new instance of LibraryModel.</returns>
    public static LibraryManagerModel CreateNewLibrary(int idLibrary)
    {
        return new LibraryManagerModel { Id = idLibrary, BookList = [] };
    }

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
        var searchBook = BookList.FirstOrDefault(b => b.Id == book.Id && b.Author == book.Author && b.Title == book.Title && b.TotalPages == book.TotalPages);

        return BookList.Remove(searchBook);
    }

    /// <summary>
    /// Sorts the book collection by author and then by title.
    /// </summary>
    public void SortLibrary()
    {
        BookList = new ObservableCollection<Book>(BookList.OrderBy(b => b.Author).ThenBy(b => b.Title));
    }

    public List<Book> FindBooksByBookElement(ILibrary library, BookElementsEnum bookElement, object partElement)
    {
        List<Book> result;
        IEnumerable<Book> tmpResult = [];
        var strElement = partElement?.ToString();

        Application.Current.Dispatcher.Invoke(new Action(() =>
        {
            switch (bookElement)
            {
                case BookElementsEnum.Author:
                    tmpResult = FindBooksByAuthor(library, strElement);
                    break;
                case BookElementsEnum.Title:
                    tmpResult = FindBooksByTitle(library, strElement);
                    break;
                case BookElementsEnum.TotalPages:
                    tmpResult = FindBooksByTotalPages(library, strElement);
                    break;
                case BookElementsEnum.PublishDate:
                    tmpResult = FindBooksByPublishDate(library, strElement);
                    break;
            }
        }));
        result = tmpResult?.ToList() ?? [];

        return result;
    }

    public IEnumerable<Book> FindBooksByAuthor(ILibrary library, string? strElement)
        => (IsNotNullOrEmpty(strElement)) ? library.BookList.Where(b => b.Author.Contains(strElement, StringComparison.OrdinalIgnoreCase)) : [];

    public IEnumerable<Book> FindBooksByTitle(ILibrary library, string? strElement)
        => (IsNotNullOrEmpty(strElement)) ? library.BookList.Where(b => b.Title.Contains(strElement, StringComparison.OrdinalIgnoreCase)) : [];

    public IEnumerable<Book> FindBooksByTotalPages(ILibrary library, string? strElement)
        => IsParseable(strElement, out var intElement) ? library.BookList.Where(b => b.TotalPages == intElement) : [];

    public IEnumerable<Book> FindBooksByPublishDate(ILibrary library, string? strElement)
        => IsParseable(strElement, out var intElement) ? library.BookList.Where(b => b.PublishDate == intElement) : [];

    /// <summary>
    /// Retrieves a collection of the first specified number of books from the library.
    /// </summary>
    /// <param name="amountFirstBooks">The number of books to retrieve.</param>
    /// <returns>A collection of the first books.</returns>
    public List<Book> GetFirstBooks(int amountFirstBooks) => BookList.Take(amountFirstBooks).ToList();

    /// <summary>
    /// Retrieves all books in the library.
    /// </summary>
    /// <returns>A collection of all books.</returns>
    public List<Book> GetAllBooks() => [.. BookList];

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
        return new LibraryManagerModel { Id = Id, BookList = new ObservableCollection<Book>(BookList.Select(b => new Book { Id = b.Id, Author = b.Author, TotalPages = b.TotalPages, Title = b.Title })) };
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
    private readonly Func<Book, string> bookSelector = b => $"{b.Id}. Author:{b.Author} - Title:{b.Title} - Pages:{b.TotalPages}";

    /// <summary>
    /// Joins a list of strings into a single string with each element separated by a comma and newline.
    /// </summary>
    /// <param name="list">The list of strings to join.</param>
    /// <returns>A single string with each element separated by a comma and newline.</returns>
    private string JoinStrings(IEnumerable<string> list) => string.Join(",\n", list);

    private bool IsParseable(string? strElement, out int intElement) => Int32.TryParse(strElement, out intElement);

    private bool IsNotNullOrEmpty(string? strElement) => !string.IsNullOrEmpty(strElement);
    #endregion
}
