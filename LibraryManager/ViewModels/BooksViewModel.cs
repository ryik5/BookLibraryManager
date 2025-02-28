using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using BookLibraryManager.Common;
using BookLibraryManager.XmlLibraryProvider.Keepers;
using BookLibraryManager.XmlLibraryProvider.Loaders;
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

        SortLibraryCommand = GetDelegateCommandWithLockAsync(SortBooks);

        AddBookCommand = GetDelegateCommandWithLockAsync(AddBook);
        DemoAddBooksCommand = GetDelegateCommandWithLockAsync(DemoAddRandomBooks);
        EditBookCommand = GetDelegateCommandWithLockAsync(EditBook);
        DeleteSelectedBooksCommand = new DelegateCommand(DeleteSelectedBooks);
        ImportBookCommand = GetDelegateCommandWithLockAsync(ImportBook);
        ExportSelectedBookCommand = GetDelegateCommandWithLockAsync(ExportSelectedBook);

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
    /// Gets or sets a value indicating whether the buttons are unlocked.
    /// </summary>
    public bool IsUnLocked
    {
        get => _isUnLocked;
        set => SetProperty(ref _isUnLocked, value);
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

    /// <summary>
    /// Command to import a book from the disk to the library.
    /// </summary>
    public DelegateCommand ImportBookCommand
    {
        get;
    }

    public string ImportBookTooltip => "Import a book from the disk to the library";

    /// <summary>
    /// Command to export the selected book to the disk.
    /// </summary>
    public DelegateCommand ExportSelectedBookCommand
    {
        get;
    }

    public string ExportSelectedBookTooltip => "Export the selected book to the disk";
    #endregion


    #region Methods
    /// <summary>
    /// Adds a new book to the library.
    /// </summary>
    private Task AddBook()
    {
        new CreatorBookDetailsViewModel(_bookManager, _settings).ShowDialog();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Adds randomly filled books to the library.
    /// </summary>
    private Task DemoAddRandomBooks()
    {
        new CreatorBookDetailsViewModel(_bookManager, _settings).AddExampleBooks(10);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Calls EditBookViewModel to edit the selected book.
    /// </summary>
    private Task EditBook()
    {
        new EditorBookDetailsViewModel(_bookManager, _settings, SelectedBook).ShowDialog();
        return Task.CompletedTask;
    }

    private async Task ExportSelectedBook()
    {
        var msg = string.Empty;
        var fileName = string.Empty;
        var pathToFile = string.Empty;
        try
        {
            var selectedFolder = new SelectionDialogHandler().GetPathToFolder(Constants.EXPORT_BOOK);
            if (string.IsNullOrEmpty(selectedFolder))
                throw new Exception(Constants.FOLDER_WAS_NOT_SELECTED);

            var window = new MessageBoxHandler();
            window.ShowInput(Constants.INPUT_BOOK_NAME, Constants.INPUT_NAME);
            if (window.DialogResult == Models.EDialogResult.YesButton && window.InputString is string bookName && !string.IsNullOrWhiteSpace(bookName))
                fileName = bookName;
            else
                fileName = SelectedBook.Id.ToString();

            pathToFile = Path.Combine(selectedFolder, StringsHandler.CreateXmlFileName(fileName));

            if (File.Exists(pathToFile))
                File.Delete(pathToFile);

            await Task.Yield();

            // XML provider of saving library
            var result = await TaskHandler.TryExecuteTaskAsync(()
                => Task.FromResult(_bookManager.TrySaveBook(new XmlBookKeeper(), SelectedBook, pathToFile)));

            msg = result?.Result ?? false ?
                $"{Constants.BOOK_WAS_SAVED_SUCCESSFULLY}: '{pathToFile}'" :
                $"{Constants.FAILED_TO_SAVE_BOOK_TO_PATH}: '{pathToFile}'";
        }
        catch
        {
            msg = $"{Constants.FAILED_TO_SAVE_BOOK_TO_PATH} '{pathToFile}'";
        }

        new MessageBoxHandler().Show(msg);
    }

    private async Task ImportBook()
    {
        var xmlFilePath = new SelectionDialogHandler().GetPathToXmlFile(Constants.IMPORT_BOOK);

        MessageHandler.PublishMessage(Constants.IMPORT_BOOK);

        // XML provider of loading library
        var result = await TaskHandler.TryExecuteTaskAsync(()
            => Task.FromResult(_bookManager.TryLoadBook(new XmlBookLoader(), xmlFilePath)));

        var msg = string.Empty;
        if (result?.Result ?? false)
        {
            var book = _bookManager.Library.BookList.Last();
            msg = $"{Constants.BOOK_WAS_IMPORTED_SUCCESSFULLY}: '{book.Title}' by {book.Author}";
        }
        else
        {
            msg = $"{Constants.FAILED_TO_IMPORT_BOOK_BY_PATH}: '{xmlFilePath}'";
        }

        new MessageBoxHandler().Show(msg);
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
    private Task SortBooks()
    {
        var props = new List<PropertyCustomInfo>();

        MakeBookCutomPropertyList(props, _settings.FirstSortBookProperty, _settings.FirstSortProperty_ByDescend);
        MakeBookCutomPropertyList(props, _settings.SecondSortBookProperty, _settings.SecondSortProperty_ByDescend);
        MakeBookCutomPropertyList(props, _settings.ThirdSortBookProperty, _settings.ThirdSortProperty_ByDescend);

        if (0 < props.Count)
            _bookManager.SafetySortBooks(props);

        MessageHandler.PublishMessage($"Library ID:{BookManager.Library.Id} was sorted");

        void MakeBookCutomPropertyList(List<PropertyCustomInfo> props, string name, bool byDescend)
        {
            var prop = _bookManager.Library.FindBookPropertyInfo(_settings.FirstSortBookProperty);
            var customProp = new PropertyCustomInfo { PropertyInfo = prop, DescendingOrder = byDescend };
            if (prop.Name != nameof(Book.None))
                props.Add(customProp);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Returns a DelegateCommand that locks the buttons while executing the specified asynchronous function.
    /// </summary>
    /// <param name="func">The asynchronous function to execute, of type Func<Task>.</param>
    /// <param name="isLocked">A boolean property that indicates whether the buttons are locked.</param>
    /// <returns>A DelegateCommand that locks the buttons while executing the specified asynchronous function.</returns>
    private DelegateCommand GetDelegateCommandWithLockAsync(Func<Task> func) => new(async () =>
    {
        try
        {
            IsUnLocked = false;
            await func().ConfigureAwait(false);
        }
        finally
        {
            IsUnLocked = true;
        }
    });

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
    private bool _isUnLocked = true;
    private Visibility _libraryVisibility;
    private Book _selectedBook;
    private ObservableCollection<Book> _selectedBooks = new();
    #endregion
}
