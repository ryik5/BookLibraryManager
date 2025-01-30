using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;
using System.Windows;
using LibraryManager.Views;

namespace LibraryManager.ViewModels;

/// <summary>
/// ViewModel for finding books in the <see cref="ILibrary"/>.
/// </summary>
/// <author>YR 2025-01-21</author>
internal class FindBookViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FindBookViewModel"/> class.
    /// </summary>
    /// <param name="libraryManager">The library manager.</param>
    public FindBookViewModel(LibraryBookManagerModel libraryManager)
    {
        _libraryManager = libraryManager;
        _statusBarKind = EWindowKind.FindBooksWindow;
        SearchOnFly = false;
        SearchFields = Enum.GetValues(typeof(BookElementsEnum)).Cast<BookElementsEnum>().ToList();
        FindBooksCommand = new RelayCommand(FindBooks, CanSearchBooks);
        EditBookCommand = new RelayCommand(EditBook, CanDeleteBook);
        RemoveBookCommand = new RelayCommand(DeleteSelectedBook, CanDeleteBook);
        CloseWindowCommand = new DelegateCommand<Window>(CloseWindow);

        StatusBarItems = new StatusBarModel(_statusBarKind);

        _finderWindow = new FindBookWindow() { DataContext = this };
        _finderWindow.ShowDialog();
    }


    #region Commands
    /// <summary>
    /// Command to find books.
    /// </summary>
    public DelegateCommand FindBooksCommand
    {
        get;
    }
    public RelayCommand EditBookCommand
    {
        get;
    }

    /// <summary>
    /// Command to delete the selected book.
    /// </summary>
    public DelegateCommand RemoveBookCommand
    {
        get;
    }

    /// <summary>
    /// Command to close the window.
    /// </summary>
    public DelegateCommand<Window> CloseWindowCommand
    {
        get;
    }
    #endregion


    #region Properties
    /// <summary>
    /// Gets the status bar items.
    /// </summary>
    public StatusBarModel StatusBarItems
    {
        get;
    }

    /// <summary>
    /// The fields of the book to perform search.
    /// </summary>
    public List<BookElementsEnum> SearchFields
    {
        get;
    }

    /// <summary>
    /// Gets or sets the list of the found books.
    /// </summary>
    public List<Book> BookList
    {
        get => _bookList;
        set => SetProperty(ref _bookList, value);
    }

    /// <summary>
    /// Gets or sets the selected book.
    /// </summary>
    public Book SelectedBook
    {
        get => _selectedBook;
        set => SetProperty(ref _selectedBook, value);
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

    /// <summary>
    /// Value indicating whether to perform search on the fly.
    /// </summary>
    public bool SearchOnFly
    {
        get => _searchOnFly;
        set => SetProperty(ref _searchOnFly, value);
    }

    /// <summary>
    /// Gets or sets the field of the book to perform search.
    /// </summary>
    public BookElementsEnum SelectedSearchField
    {
        get => _selectedSearchField;
        set => SetProperty(ref _selectedSearchField, value);
    }

    /// <summary>
    /// Text log of operations.
    /// </summary>
    public string TextLog
    {
        get => _textLog;
        set => SetProperty(ref _textLog, value);
    }
    #endregion


    #region private methods
    /// <summary>
    /// Finds books based on the search text. Updates <see cref="BookList"/>.
    /// </summary>
    private void FindBooks()
    {
        BookList = _libraryManager.FindBooksByBookElement(SelectedSearchField, SearchText);
        var totalBooks = _libraryManager.TotalBooks;
        var foundBooks = BookList.Count;
        MessageHandler.SendToStatusBar(_statusBarKind, $"Looked for {SelectedSearchField}:{SearchText}. Found {foundBooks} from {totalBooks}");
    }

    private void EditBook()
    {
        var editBookView = new EditBookViewModel(_libraryManager, SelectedBook);
        editBookView.ShowDialog();
        RaisePropertyChanged(nameof(_libraryManager.BookList));
        RaisePropertyChanged(nameof(BookList));
        if (editBookView.Book is Book book)
            MessageHandler.SendToStatusBar(_statusBarKind, $"Last edited book was '{book.Title}");
    }

    /// <summary>
    /// Deletes the selected book.
    /// </summary>
    private void DeleteSelectedBook()
    {
        var text = _libraryManager.RemoveBook(SelectedBook) ? "Book was deleted successfully" : "Nothing to delete";
        BookList = _libraryManager.FindBooksByBookElement(SelectedSearchField, SearchText);
        TextLog = text;
        MessageHandler.SendToStatusBar(_statusBarKind, text);
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
    #endregion


    #region private fields
    private readonly FindBookWindow _finderWindow;
    private readonly LibraryBookManagerModel _libraryManager;
    private readonly EWindowKind _statusBarKind;
    private List<Book> _bookList;
    private Book _selectedBook;
    private string _searchText;
    private bool _searchOnFly;
    private BookElementsEnum _selectedSearchField;
    private string _textLog;
    #endregion
}
