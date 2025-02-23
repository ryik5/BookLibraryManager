using System.Reflection;
using System.Windows;
using BookLibraryManager.Common.Util;

namespace BookLibraryManager.Common;

/// <summary>
/// Represents a model for managing books in a library.
/// </summary>
/// <author>YR 2025-01-09</author>
public class BookManagerModel : BindableBase, IBookManageable
{
    public BookManagerModel(ILibrary library)
    {
        if (library is null)
            throw new ArgumentNullException(nameof(library));

        _library = library;
        RaisePropertyChanged(nameof(Library));
    }


    #region public methods
    /// <summary>
    /// Loads a book from the specified file path.
    /// </summary>
    /// <param name="bookLoader">The loader responsible for loading the book.</param>
    /// <param name="pathToFile">The path to the file containing the book data.</param>
    /// <returns>True if the book was successfully loaded; otherwise, false.</returns>
    public bool TryLoadBook(IBookLoader bookLoader, string pathToFile)
    {
        bookLoader.LoadingFinished += BookLoader_LoadingBookFinished;

        var result = bookLoader.TryLoadBook(pathToFile, out var book);
        if (result)
            AddBook(book);

        bookLoader.LoadingFinished -= BookLoader_LoadingBookFinished;

        return result;
    }

    /// <summary>
    /// Saves the selected book to the specified folder.
    /// </summary>
    /// <param name="keeper">The keeper responsible for saving the book.</param>
    /// <param name="pathToFolder">The path to the folder where the book will be saved.</param>
    /// <returns>True if the book was successfully saved; otherwise, false.</returns>
    public bool TrySaveBook(IBookKeeper keeper, Book book, string pathToFolder)
        => keeper.TrySaveBook(book, pathToFolder);

    /// <summary>
    /// Sorts the book collection by author and then by title.
    /// </summary>
    public void SortBooks()
    {
        InvokeOnUiThread(() =>
        Library.BookList.ResetAndAddRange(Library.BookList
            .OrderBy(b => b.Author)
            .ThenBy(b => b.Title)));
    }

    public void SafetySortBooks(List<PropertyInfo> sortProperties)
    {
        InvokeOnUiThread(() =>
        Library.BookList.ResetAndAddRange(GetSortedBookList(sortProperties)));
    }

    public PropertyInfo[] GetBookProperties()
    {
        var bookType = typeof(Book);
        return bookType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    /// <summary>
    /// Adds a book to the library.
    /// </summary>
    /// <param name="book">The book to add.</param>
    public void AddBook(Book book)
    {
        var max = Library.BookList.Count == 0 ? 0 : Library.BookList.Max(b => b.Id);
        book.Id = max + 1;
        Library.BookList.Add(book);
    }

    /// <summary>
    /// Removes a book from the library.
    /// </summary>
    /// <param name="book">The book to remove.</param>
    /// <returns>True if the book was successfully removed; otherwise, false.</returns>
    public bool TryRemoveBook(Book book)
    {
        var result = Library.BookList.RemoveItem(book);

        return result;
    }

    /// <summary>
    /// Finds books in the library by a specified book element.
    /// </summary>
    /// <param name="bookElement">The element of the book to search by.</param>
    /// <param name="partElement">The value of the element to search for.</param>
    /// <returns>A list of books that match the search criteria.</returns>
    public List<Book> FindBooksByKind(EBibliographicKindInformation bookElement, object partElement)
    {
        IEnumerable<Book> tmpResult = [];
        var strElement = partElement?.ToString();

        InvokeOnUiThread(() => tmpResult = FindBooks(bookElement, strElement));

        return tmpResult?.ToList() ?? [];
    }
    #endregion


    #region Properties
    /// <summary>
    /// Gets or sets a library.
    /// </summary>
    public ILibrary Library
    {
        get => _library;
        set => SetProperty(ref _library, value);
    }

    public event EventHandler<ActionFinishedEventArgs> LoadingFinished;
    #endregion


    #region private methods
    /// <summary>
    /// Returns a sorted list of books based on the provided properties.
    /// </summary>
    /// <param name="sortProperties">The properties to sort the books by.</param>
    /// <returns>A sorted list of Book objects.</returns>
    private IEnumerable<Book> GetSortedBookList(List<PropertyInfo> sortProperties)
    {
        var orderedBooks = Library.BookList.Where(b => 0 < b.Id);

        foreach (var property in sortProperties)
        {
            if (orderedBooks is IOrderedEnumerable<Book>)
            {
                orderedBooks = ((IOrderedEnumerable<Book>)orderedBooks).ThenBy(b => property.GetValue(b));
            }
            else
            {
                orderedBooks = orderedBooks.OrderBy(b => property.GetValue(b));
            }
        }

        return orderedBooks;
    }

    private void BookLoader_LoadingBookFinished(object? sender, ActionFinishedEventArgs e)
    {
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = e.Message, IsFinished = e.IsFinished });
    }

    /// <summary>
    /// Finds books in the library by a specified book element.
    /// </summary>
    /// <param name="bookElement">The element of the book to search by.</param>
    /// <param name="strElement">The value of the element to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    private IEnumerable<Book> FindBooks(EBibliographicKindInformation bookElement, string? strElement)
    {
        IEnumerable<Book> tmpResult = [];
        switch (bookElement)
        {
            case EBibliographicKindInformation.Author:
                tmpResult = FindBooksByAuthor(strElement);
                break;
            case EBibliographicKindInformation.Title:
                tmpResult = FindBooksByTitle(strElement);
                break;
            case EBibliographicKindInformation.TotalPages:
                tmpResult = FindBooksByTotalPages(strElement);
                break;
            case EBibliographicKindInformation.PublishDate:
                tmpResult = FindBooksByPublishDate(strElement);
                break;
            default:
                tmpResult = FindBooksAnyWhere(strElement);
                break;
        }
        return tmpResult;
    }

    /// <summary>
    /// Finds books in the library by author.
    /// </summary>
    /// <param name="strElement">The author to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    private IEnumerable<Book> FindBooksByAuthor(string? strElement) => IsNotNullOrEmpty(strElement)
        ? Library.BookList.Where(b => b.Author.Contains(strElement, CurrentComparisionRule))
        : [];

    /// <summary>
    /// Finds books in the library by title.
    /// </summary>
    /// <param name="strElement">The title to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    private IEnumerable<Book> FindBooksByTitle(string? strElement)
        => IsNotNullOrEmpty(strElement)
        ? Library.BookList.Where(b => b.Title.Contains(strElement, CurrentComparisionRule))
        : [];

    /// <summary>
    /// Finds books in the library by total pages.
    /// </summary>
    /// <param name="strElement">The total pages to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    private IEnumerable<Book> FindBooksByTotalPages(string? strElement)
        => IsParseable(strElement, out var intElement)
        ? Library.BookList.Where(b => b.TotalPages == intElement)
        : [];

    /// <summary>
    /// Finds books in the library by publish date.
    /// </summary>
    /// <param name="strElement">The publish date to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    private IEnumerable<Book> FindBooksByPublishDate(string? strElement)
        => IsParseable(strElement, out var intElement)
        ? Library.BookList.Where(b => b.PublishDate == intElement)
        : [];

    private IEnumerable<Book> FindBooksAnyWhere(string? strElement)
    {
        IEnumerable<Book> tmpResult = [];
        var isInt = IsParseable(strElement, out var intElement);
        var isString = IsNotNullOrEmpty(strElement);

        if (isInt)
            tmpResult = Library.BookList.Where(b =>
            b.TotalPages == intElement ||
            b.PublishDate == intElement ||
            (b.Author?.Contains(strElement, CurrentComparisionRule) ?? false) ||
            (b.Title?.Contains(strElement, CurrentComparisionRule) ?? false));
        else
            tmpResult = isString ? Library.BookList.Where(b =>
            (b.Author?.Contains(strElement, CurrentComparisionRule) ?? false) ||
            (b.Description?.Contains(strElement, CurrentComparisionRule) ?? false) ||
            (b.Genre?.Contains(strElement, CurrentComparisionRule) ?? false) ||
            (b.ISBN?.Contains(strElement, CurrentComparisionRule) ?? false) ||
            (b.Title?.Contains(strElement, CurrentComparisionRule) ?? false)) : [];

        return tmpResult ?? [];
    }

    /// <summary>
    /// Determines whether the specified string can be parsed to an integer.
    /// </summary>
    /// <param name="strElement">The string to parse.</param>
    /// <param name="intElement">The parsed integer value.</param>
    /// <returns>True if the string can be parsed to an integer; otherwise, false.</returns>
    private bool IsParseable(string? strElement, out int intElement) => int.TryParse(strElement, out intElement);

    /// <summary>
    /// Determines whether the specified string is not null or empty.
    /// </summary>
    /// <param name="strElement">The string to check.</param>
    /// <returns>True if the string is not null or empty; otherwise, false.</returns>
    private bool IsNotNullOrEmpty(string? strElement) => !string.IsNullOrEmpty(strElement);

    private void InvokeOnUiThread(Action action) => Application.Current?.Dispatcher?.Invoke(action);

    private const StringComparison CurrentComparisionRule = StringComparison.OrdinalIgnoreCase;

    private ILibrary _library;
    #endregion
}
