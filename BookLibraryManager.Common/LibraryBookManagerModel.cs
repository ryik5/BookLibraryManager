using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Serialization;

namespace BookLibraryManager.Common;

/// <summary>
/// Represents a model for managing a library of books.
/// </summary>
/// <author>YR 2025-01-09</author>
public class LibraryBookManagerModel : LibraryAbstract, ILibrary
{
    /// <summary>
    /// Creates a new library with the specified ID.
    /// </summary>
    /// <param name="idLibrary">The ID of the new library.</param>
    public void CreateNewLibrary(int idLibrary)
    {
        Id = idLibrary;
        BookList = [];
    }

    /// <summary>
    /// Loads a library from the specified file path.
    /// </summary>
    /// <param name="libraryLoader">The loader responsible for loading the library.</param>
    /// <param name="pathToFile">The path to the file containing the library data.</param>
    /// <returns>True if the library was successfully loaded; otherwise, false.</returns>
    public bool LoadLibrary(ILibraryLoader libraryLoader, string pathToFile)
    {
        libraryLoader.LoadingFinished += LibraryLoader_LoadingLibraryFinished;

        var result = libraryLoader.TryLoadLibrary(pathToFile, out var library);
        if (result)
        {
            BookList = new ObservableCollection<Book>(library.BookList);
            Id = library.Id;
        }
        else
        {
            BookList = [];
            Id = 0;
        }
        libraryLoader.LoadingFinished -= LibraryLoader_LoadingLibraryFinished;

        return result;
    }

    /// <summary>
    /// Saves the specified library to the specified folder.
    /// </summary>
    /// <param name="keeper">The keeper responsible for saving the library.</param>
    /// <param name="pathToFolder">The path to the folder where the library will be saved.</param>
    /// <returns>True if the library was successfully saved; otherwise, false.</returns>
    public bool SaveLibrary(ILibraryKeeper keeper, string pathToFolder) => keeper.SaveLibrary(this, pathToFolder);

    /// <summary>
    /// Sorts the book collection by author and then by title.
    /// </summary>
    public void SortLibrary()
    {
        BookList = new ObservableCollection<Book>(BookList.OrderBy(b => b.Author).ThenBy(b => b.Title));
    }

    public void CloseLibrary()
    {
        BookList = null;
        Id = -1;
    }

    /// <summary>
    /// Adds a book to the library.
    /// </summary>
    /// <param name="book">The book to add.</param>
    public void AddBook(Book book)
    {
        var max = BookList.Count == 0 ? 0 : BookList.Max(b => b.Id);
        book.Id = max + 1;
        BookList.Add(book);
    }

    /// <summary>
    /// Removes a book from the library.
    /// </summary>
    /// <param name="book">The book to remove.</param>
    /// <returns>True if the book was successfully removed; otherwise, false.</returns>
    public bool RemoveBook(Book book)
    {
        if (book is null)
            return false;

        var searchBook = BookList.FirstOrDefault(b => b.Id == book.Id && b.Author == book.Author && b.Title == book.Title && b.TotalPages == book.TotalPages && b.PublishDate == book.PublishDate);

        return searchBook != null && BookList.Remove(searchBook);
    }

    /// <summary>
    /// Finds books in the library by a specified book element.
    /// </summary>
    /// <param name="bookElement">The element of the book to search by.</param>
    /// <param name="partElement">The value of the element to search for.</param>
    /// <returns>A list of books that match the search criteria.</returns>
    public List<Book> FindBooksByBookElement(BookElementsEnum bookElement, object partElement)
    {
        IEnumerable<Book> tmpResult = [];
        var strElement = partElement?.ToString();

        if (Application.Current is null)
        {
            tmpResult = FindBooks(bookElement, strElement);
        }
        else
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                tmpResult = FindBooks(bookElement, strElement);
            }));
        }

        return tmpResult?.ToList() ?? [];
    }

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
    public List<Book> GetAllBooks() => BookList.ToList();

    /// <summary>
    /// Gets or sets the selected book.
    /// </summary>
    [XmlIgnore]
    public Book SelectedBook
    {
        get => _selectedBook;
        set => SetProperty(ref _selectedBook, value);
    }
    private Book _selectedBook;

    public event EventHandler<ActionFinishedEventArgs> LoadingFinished;


    #region private methods
    private void LibraryLoader_LoadingLibraryFinished(object? sender, ActionFinishedEventArgs e)
    {
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = e.Message, IsFinished = e.IsFinished });
    }

    /// <summary>
    /// Finds books in the library by a specified book element.
    /// </summary>
    /// <param name="bookElement">The element of the book to search by.</param>
    /// <param name="strElement">The value of the element to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    private IEnumerable<Book> FindBooks(BookElementsEnum bookElement, string? strElement)
    {
        IEnumerable<Book> tmpResult = [];
        switch (bookElement)
        {
            case BookElementsEnum.Author:
                tmpResult = FindBooksByAuthor(strElement);
                break;
            case BookElementsEnum.Title:
                tmpResult = FindBooksByTitle(strElement);
                break;
            case BookElementsEnum.TotalPages:
                tmpResult = FindBooksByTotalPages(strElement);
                break;
            case BookElementsEnum.PublishDate:
                tmpResult = FindBooksByPublishDate(strElement);
                break;
            default:
                tmpResult = FindBooksEveryWhere(strElement);
                break;
        }
        return tmpResult;
    }

    /// <summary>
    /// Finds books in the library by author.
    /// </summary>
    /// <param name="strElement">The author to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    private IEnumerable<Book> FindBooksByAuthor(string? strElement)
        => IsNotNullOrEmpty(strElement)
        ? BookList.Where(b => b.Author.Contains(strElement, CurrentComparisionRule))
        : [];

    /// <summary>
    /// Finds books in the library by title.
    /// </summary>
    /// <param name="strElement">The title to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    private IEnumerable<Book> FindBooksByTitle(string? strElement)
        => IsNotNullOrEmpty(strElement)
        ? BookList.Where(b => b.Title.Contains(strElement, CurrentComparisionRule))
        : [];

    /// <summary>
    /// Finds books in the library by total pages.
    /// </summary>
    /// <param name="strElement">The total pages to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    private IEnumerable<Book> FindBooksByTotalPages(string? strElement)
        => IsParseable(strElement, out var intElement)
        ? BookList.Where(b => b.TotalPages == intElement)
        : [];

    /// <summary>
    /// Finds books in the library by publish date.
    /// </summary>
    /// <param name="strElement">The publish date to search for.</param>
    /// <returns>An enumerable collection of books that match the search criteria.</returns>
    private IEnumerable<Book> FindBooksByPublishDate(string? strElement)
        => IsParseable(strElement, out var intElement)
        ? BookList.Where(b => b.PublishDate == intElement)
        : [];

    private IEnumerable<Book> FindBooksEveryWhere(string? strElement)
    {
        IEnumerable<Book> tmpResult = [];
        var isInt = IsParseable(strElement, out var intElement);
        var isString = IsNotNullOrEmpty(strElement);

        if (isInt)
            tmpResult = BookList.Where(b =>
            b.TotalPages == intElement ||
            b.PublishDate == intElement ||
            (b.Author?.Contains(strElement, CurrentComparisionRule) ?? false) ||
            (b.Title?.Contains(strElement, CurrentComparisionRule) ?? false));
        else
            tmpResult = isString ? BookList.Where(b =>
            (b.Author?.Contains(strElement, CurrentComparisionRule) ?? false) ||
            (b.Description?.Contains(strElement, CurrentComparisionRule) ?? false) ||
            (b.Genre?.Contains(strElement, CurrentComparisionRule) ?? false) ||
            (b.ISDN?.Contains(strElement, CurrentComparisionRule) ?? false) ||
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

    private StringComparison CurrentComparisionRule = StringComparison.OrdinalIgnoreCase;
    #endregion
}
