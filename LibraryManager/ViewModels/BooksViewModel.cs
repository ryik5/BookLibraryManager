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
    public BooksViewModel(IBookManageable bookManager)
    {
        _bookManager = bookManager;
        SortLibraryCommand = new DelegateCommand(SortBooks);

        AddBookCommand = new DelegateCommand(AddBook);
        DemoAddBooksCommand = new DelegateCommand(DemoAddRandomBooks);
        RemoveBookCommand = new DelegateCommand(DeleteSelectedBook);
        EditBookCommand = new DelegateCommand(EditBook);

        LibraryVisibility = Visibility.Collapsed;
        _bookManager.Library.LibraryIdChanged += Library_LibraryIdChanged;
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
    private Book _selectedBook;
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
    /// Command to delete a book from the library.
    /// </summary>
    public DelegateCommand RemoveBookCommand
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
    /// Deletes a book from the library.
    /// </summary>
    private void DeleteSelectedBook()
    {
        var text = _bookManager.TryRemoveBook(SelectedBook) ? Constants.BOOK_WAS_DELETED_SUCCESSFULLY : Constants.NO_BOOKS_FOUND;

        MessageHandler.PublishMessage(text);
        MessageHandler.PublishTotalBooksInLibrary(_bookManager.Library.TotalBooks);
    }

    /// <summary>
    /// Sorts the books in the library.
    /// </summary>
    private void SortBooks()
    {
        _bookManager.SortBooks();
        MessageHandler.PublishMessage($"Library ID:{BookManager.Library.Id} was sorted");
    }

    /// <summary>
    /// Handles the LibraryIdChanged event by updating the CanOperateWithBooks property.
    /// </summary>
    private void Library_LibraryIdChanged(object? sender, EventArgs e)
    {
        CanOperateWithBooks = (sender as ILibrary)?.Id != 0;
    }
    #endregion


    #region Private Members
    private readonly IBookManageable _bookManager;
    private bool _isChecked;
    private bool _canEditBook;
    private bool _canOperateWithBooks;
    private Visibility _libraryVisibility;
    #endregion
}
