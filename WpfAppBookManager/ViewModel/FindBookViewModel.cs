using System.Windows;
using BookLibraryManager.Common;
using BookLibraryManager.TestApp.View;
using GalaSoft.MvvmLight.CommandWpf;

namespace BookLibraryManager.TestApp.ViewModel;

/// <summary>
/// ViewModel for finding books in the <see cref="ILibrary"/>.
/// </summary>
/// <author>YR 2025-01-21</author>
public class FindBookViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FindBookViewModel"/> class.
    /// </summary>
    /// <param name="libraryManager">The library manager.</param>
    /// <param name="library">The library.</param>
    public FindBookViewModel(LibraryBookManagerModel libraryManager)
    {
        _libraryManager = libraryManager;
        SearchOnFly = false;
        SearchFields = Enum.GetValues(typeof(BookElementsEnum)).Cast<BookElementsEnum>().ToList();
        FindBooksCommand = new RelayCommand(FindBooks, CanSearchBooks);
        DeleteSelectedBookCommand = new RelayCommand(DeleteSelectedBook, CanDeleteBook);
        CloseWindowCommand = new RelayCommand<Window>(CloseWindow);
        _finderWindow = new FindBookWindow() { DataContext = this };
        _finderWindow.ShowDialog();
    }

    /// <summary>
    /// Command to find books.
    /// </summary>
    public RelayCommand FindBooksCommand
    {
        get;
    }

    /// <summary>
    /// Finds books based on the search text. Updates <see cref="BookList"/>
    /// </summary>
    private void FindBooks()
    {
        BookList = _libraryManager.FindBooksByBookElement( SelectedSearchField, SearchText);
    }

    /// <summary>
    /// The search text.
    /// </summary>
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value) && SearchOnFly)
                FindBooks();
        }
    }
    private string _searchText;

    private bool _searchOnFly;
    /// <summary>
    /// Value indicating whether to perform search on the fly.
    /// </summary>
    public bool SearchOnFly
    {
        get => _searchOnFly;
        set => SetProperty(ref _searchOnFly, value);
    }

    /// <summary>
    /// the fields of the book to perform search.
    /// </summary>
    public List<BookElementsEnum> SearchFields{
        get;
    }

    /// <summary>
    /// Gets or sets the field of the book to perform search.
    /// </summary>
    public BookElementsEnum SelectedSearchField
    {
        get => _selectedSearchField;
        set => SetProperty(ref _selectedSearchField, value);
    }
    private BookElementsEnum _selectedSearchField;

    /// <summary>
    /// Text log of operations
    /// </summary>
    public string TextLog
    {
        get => _textLog;
        set => SetProperty(ref _textLog, value);
    }
    private string _textLog;

    /// <summary>
    /// Gets or sets the list of books.
    /// </summary>
    public List<Book> BookList
    {
        get => _bookList;
        set => SetProperty(ref _bookList, value);
    }
    private List<Book> _bookList;

    /// <summary>
    /// Gets or sets the selected book.
    /// </summary>
    public Book SelectedBook
    {
        get => _selectedBook;
        set => SetProperty(ref _selectedBook, value);
    }
    private Book _selectedBook;

    /// <summary>
    /// Command to close the window.
    /// </summary>
    public RelayCommand<Window> CloseWindowCommand
    {
        get;
    }
    /// <summary>
    /// Closes the specified window.
    /// </summary>
    private void CloseWindow(Window window)
    {
        window?.Close();
        _finderWindow?.Close();
    }

    /// <summary>
    /// Command to Delete the selected book.
    /// </summary>
    public RelayCommand DeleteSelectedBookCommand
    {
        get;
    }
    /// <summary>
    /// Deletes the selected book.
    /// </summary>
    private void DeleteSelectedBook()
    {
        TextLog = _libraryManager.RemoveBook( SelectedBook) ? "Book was deleted successfully" : "Nothing to delete";
        BookList = _libraryManager.FindBooksByBookElement(SelectedSearchField, SearchText);
    }

    /// <summary>
    /// Determines whether a book can be deleted.
    /// </summary>
    /// <returns>true if a book is selected; otherwise, false.</returns>
    public bool CanDeleteBook()
    {
        return SelectedBook is Book;
    }

    /// <summary>
    /// Determines whether operations can be performed on the books in the library.
    /// </summary>
    /// <returns>true if the library has a book list; otherwise, false.</returns>
    private bool CanSearchBooks()
    {
        return _libraryManager?.BookList != null;
    }

    private readonly FindBookWindow _finderWindow;
    private readonly LibraryBookManagerModel _libraryManager;
}
