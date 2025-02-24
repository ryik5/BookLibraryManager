using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;

namespace LibraryManager.ViewModels;

/// <summary>
/// Represents a view model for managing books in a library.
/// </summary>
/// <author>YR 2025-02-02</author>
internal sealed class BooksViewModel : BindableBase, IViewModelPageable
{
    /// <summary>
    /// Initializes a new instance of the BooksViewModel class.
    /// </summary>
    /// <param name="bookManager">The book manager model.</param>
    public BooksViewModel(IBookManageable bookManager, SettingsModel settings)
    {
        _bookManager = bookManager;
        _settings = settings;
        SortLibraryCommand = new DelegateCommand(SortBooks);

        AddBookCommand = new DelegateCommand(AddBook);
        DemoAddBooksCommand = new DelegateCommand(DemoAddRandomBooks);
        DeleteSelectedBooksCommand = new DelegateCommand(DeleteSelectedBooks);
        EditBookCommand = new DelegateCommand(EditBook);

        LibraryVisibility = Visibility.Collapsed;
        _bookManager.Library.LibraryIdChanged += Handle_LibraryIdChanged;
    }


    #region Properties
    public string Name => Constants.BOOKS;

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }

    /// <summary>
    /// Determines whether a book can be edited.
    /// </summary>
    /// <returns>true if the library has a book list and the selected book is not null; otherwise, false.</returns>
    public bool CanEditBook
    {
        get => _canEditBook;
        set => SetProperty(ref _canEditBook, value);
    }

    public bool CanOperateWithBooks
    {
        get => _canOperateWithBooks;
        set => SetProperty(ref _canOperateWithBooks, value);
    }

    /// <summary>
    /// Gets the library manager model.
    /// </summary>
    public IBookManageable BookManager => _bookManager;

    /// <summary>
    /// Gets or sets the visibility of the library's table.
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
        set
        {
            if (SetProperty(ref _selectedBook, value) && value is Book)
            {
                CanEditBook = 0 < _bookManager.Library.TotalBooks;
            }
        }
    }

    /// <summary>
    /// Gets or sets the collection of the selected books.
    /// </summary>
    public ObservableCollection<Book> SelectedBooks
    {
        get => _selectedBooks;
        set => SetProperty(ref _selectedBooks, value);
    }
    #endregion


    #region Commands
    /// <summary>
    /// Command to sort the books in the library.
    /// </summary>
    public DelegateCommand SortLibraryCommand
    {
        get;
    }

    public string SortLibraryTooltip => "Sort the library by author then title";

    /// <summary>
    /// Command to add a new book to the library.
    /// </summary>
    public DelegateCommand AddBookCommand
    {
        get;
    }

    public string AddBookTooltip => "Add a new book to the library";

    /// <summary>
    /// Command to add random books to the library.
    /// </summary>
    public DelegateCommand DemoAddBooksCommand
    {
        get;
    }

    public string DemoAddBooksTooltip => "Add 10 randomly filled books to the library";

    /// <summary>
    /// Command to edit a book in the library.
    /// </summary>
    public DelegateCommand EditBookCommand
    {
        get;
    }

    public string EditBookTooltip => "Edit the selected book";

    /// <summary>
    /// Command to delete selected books from the library.
    /// </summary>
    public DelegateCommand DeleteSelectedBooksCommand
    {
        get;
    }

    public string RemoveBookTooltip => "Delete the selected book from the library";
    #endregion


    #region Methods
    /// <summary>
    /// Adds a new book to the library.
    /// </summary>
    private void AddBook()
    {
        new CreatorBookDetailsViewModel(_bookManager).ShowDialog();
    }

    /// <summary>
    /// Adds randomly filled books to the library.
    /// </summary>
    private void DemoAddRandomBooks()
    {
        new CreatorBookDetailsViewModel(_bookManager).AddExampleBooks(10);
    }

    /// <summary>
    /// Calls EditBookViewModel to edit the selected book.
    /// </summary>
    private void EditBook()
    {
        new EditorBookDetailsViewModel(_bookManager, SelectedBook).ShowDialog();
    }

    /// <summary>
    /// Deletes selected books from the library.
    /// </summary>
    private void DeleteSelectedBooks()
    {
        var booksToDelete = SelectedBooks.ToList();
        var text = string.Empty;

        foreach (var book in booksToDelete)
        {
            var id = book.Id;
            text = _bookManager.TryRemoveBook(book) ? $"{Constants.BOOK_WAS_DELETED_SUCCESSFULLY} {id}" : Constants.NO_BOOKS_FOUND;
            MessageHandler.PublishMessage(text);
        }

        MessageHandler.PublishTotalBooksInLibrary(_bookManager.Library.TotalBooks);
    }

    /// <summary>
    /// Sorts the books in the library.
    /// </summary>
    private void SortBooks()
    {
        var props = new List<PropertyInfo>();

        AddBookPropertyToList(props, _settings.FirstSortBookProperty);
        AddBookPropertyToList(props, _settings.SecondSortBookProperty);
        AddBookPropertyToList(props, _settings.ThirdSortBookProperty);

        if (0 < props.Count)
            _bookManager.SafetySortBooks(props);

        MessageHandler.PublishMessage($"Library ID:{BookManager.Library.Id} was sorted");

        void AddBookPropertyToList(List<PropertyInfo> props, string name)
        {
            var prop = _bookManager.Library.FindBookPropertyInfo(_settings.FirstSortBookProperty);
            if (prop.Name != nameof(Book.None))
                props.Add(prop);
        }
    }


    /// <summary>
    /// Handles the LibraryIdChanged event by updating the CanOperateWithBooks property.
    /// </summary>
    private void Handle_LibraryIdChanged(object? sender, EventArgs e)
    {
        CanOperateWithBooks = (sender as ILibrary)?.Id != 0;
    }
    #endregion


    #region Private Members
    private readonly IBookManageable _bookManager;
    private SettingsModel _settings;
    private bool _isChecked;
    private bool _canEditBook;
    private bool _canOperateWithBooks;
    private Visibility _libraryVisibility;
    private Book _selectedBook;
    private ObservableCollection<Book> _selectedBooks = new();
    #endregion
}
