using System.Windows;
using BookLibraryManager.Common;
using BookLibraryManager.TestApp.View;
using GalaSoft.MvvmLight.CommandWpf;

namespace BookLibraryManager.TestApp.ViewModel;

public class FindBookViewModel : BindableBase
{
    public FindBookViewModel(BookLibraryManager libraryManager, ILibrary library)
    {
        _libraryManager = libraryManager;
        _library = library;
        SearchOnFly = false;
        SearchFields = ["Title", "Author", "Pages"];
        FindBooksCommand = new RelayCommand(FindBooks, CanSearchBooks);
        DeleteSelectedBookCommand = new RelayCommand(DeleteSelectedBook, CanDeleteBook);
        CloseWindowCommand = new RelayCommand<Window>(CloseWindow);

        _finderWindow = new FindBookWindow() { DataContext = this };
        _finderWindow.ShowDialog();
    }

    public RelayCommand FindBooksCommand
    {
        get;
    }
    private void FindBooks()
    {
        BookList = _libraryManager.FindBooksByTitle(_library, SearchText);
    }

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value) && SearchOnFly)
                FindBooks();
        }
    }

    private bool _searchOnFly;
    public bool SearchOnFly
    {
        get => _searchOnFly;
        set => SetProperty(ref _searchOnFly, value);
    }

    private List<string> _searchFields;
    public List<string> SearchFields
    {
        get => _searchFields;
        set => SetProperty(ref _searchFields, value);
    }

    private string _selectedSearchField;
    public string SelectedSearchField
    {
        get => _selectedSearchField;
        set => SetProperty(ref _selectedSearchField, value);
    }

    private string _textLog;
    public string TextLog
    {
        get => _textLog;
        set => SetProperty(ref _textLog, value);
    }

    private List<Book> _bookList;
    public List<Book> BookList
    {
        get => _bookList;
        set => SetProperty(ref _bookList, value);
    }

    private Book _selectedBook;
    public Book SelectedBook
    {
        get => _selectedBook;
        set => SetProperty(ref _selectedBook, value);
    }

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

    public RelayCommand DeleteSelectedBookCommand
    {
        get;
    }
    private void DeleteSelectedBook()
    {
        TextLog = _libraryManager.RemoveBook(_library, SelectedBook) ? "Book was deleted successfully" : "Nothing to delete";
        BookList = _libraryManager.FindBooksByTitle(_library, SearchText);
    }

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
        return _library?.BookList != null;
    }


    private FindBookWindow _finderWindow;
    private readonly BookLibraryManager _libraryManager;
    private readonly ILibrary _library;
}