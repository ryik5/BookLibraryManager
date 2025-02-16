using System.Windows;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;

namespace LibraryManager.ViewModels;

/// <summary>
/// ViewModel for finding books in the <see cref="ILibrary"/>.
/// </summary>
/// <author>YR 2025-01-21</author>
internal sealed class FindBookViewModel : BindableBase, IViewModelPageable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FindBookViewModel"/> class.
    /// </summary>
    /// <param name="libraryManager">The library manager.</param>
    public FindBookViewModel(IBookManageable libraryManager, SettingsModel settings)
    {
        _settings = settings;
        _libraryManager = libraryManager;

        LibraryVisibility = Visibility.Collapsed;
        SearchFields = Enum.GetValues(typeof(EBibliographicKindInformation)).Cast<EBibliographicKindInformation>().ToList();
        RaisePropertyChanged(nameof(SearchOnFly));
        RaisePropertyChanged(nameof(SelectedSearchField));

        FindBooksCommand = new RelayCommand(FindBooks, CanSearchBooks);
        EditBookCommand = new RelayCommand(EditBook, CanDeleteBook);
        RemoveBookCommand = new RelayCommand(DeleteSelectedBook, CanDeleteBook);
    }


    #region Properties
    public string Name => Constants.FIND_BOOKS;

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetProperty(ref _isEnabled, value);
    }

    /// <summary>
    /// The fields of the book to perform search.
    /// </summary>
    public List<EBibliographicKindInformation> SearchFields
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
    /// Gets or sets the visibility of the Library's table.
    /// </summary>
    public Visibility LibraryVisibility
    {
        get => _libraryVisibility;
        set => SetProperty(ref _libraryVisibility, value);
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
        get => _settings.SearchOnFly;
        set
        {
            if (SetProperty(ref _settings.SearchOnFly, value) && value && !string.IsNullOrEmpty(SearchText))
                FindBooks();
        }
    }

    /// <summary>
    /// Gets or sets the field of the book to perform search.
    /// </summary>
    public EBibliographicKindInformation SelectedSearchField
    {
        get => _settings.SearchField;
        set
        {
            if (SetProperty(ref _settings.SearchField, value) && SearchOnFly && !string.IsNullOrEmpty(SearchText))
                FindBooks();
        }
    }
    #endregion


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


    #region private methods
    /// <summary>
    /// Finds books based on the search text. Updates <see cref="BookList"/>.
    /// </summary>
    private void FindBooks()
    {
        BookList = _libraryManager.FindBooksByKind(SelectedSearchField, SearchText);
        var foundBooks = BookList.Count;

        LibraryVisibility = BookList?.Count < 1 ? Visibility.Collapsed : Visibility.Visible;

        MessageHandler.PublishMessage($"Looked for {SelectedSearchField}:{SearchText}. Found {foundBooks}");
    }

    /// <summary>
    /// Call EditBookViewModel to edit of the SelectedBook.
    /// </summary>
    private void EditBook()
    {
        var editBookView = new EditorBookDetailsViewModel(_libraryManager, SelectedBook);
        editBookView.ShowDialog();
        RaisePropertyChanged(nameof(BookList));
        if (editBookView.Book is Book book)
            MessageHandler.PublishMessage($"Last edited book was '{book.Title}");
    }

    /// <summary>
    /// Deletes the selected book.
    /// </summary>
    private void DeleteSelectedBook()
    {
        var text = _libraryManager.TryRemoveBook(SelectedBook) ? "Book was deleted successfully" : "Nothing to delete";
        BookList = _libraryManager.FindBooksByKind(SelectedSearchField, SearchText);

        MessageHandler.PublishMessage(text);
        MessageHandler.PublishTotalBooksInLibrary(_libraryManager.Library.TotalBooks);
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
        return _libraryManager.Library?.BookList != null;
    }
    #endregion


    #region private fields
    private readonly IBookManageable _libraryManager;
    private SettingsModel _settings;
    private List<Book> _bookList;
    private Book _selectedBook;
    private string _searchText;
    private bool _isChecked;
    private bool _isEnabled = true;
    private Visibility _libraryVisibility;
    #endregion
}
