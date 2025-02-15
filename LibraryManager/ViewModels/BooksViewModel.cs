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
    /// <param name="libraryManager">The library manager model.</param>
    public BooksViewModel(IBookManageable libraryManager)
    {
        _bookManager = libraryManager;
        SortLibraryCommand = new RelayCommand(SortBooks, CanOperateWithBooks);

        AddBookCommand = new RelayCommand(AddBook, CanOperateWithBooks);
        DemoAddBooksCommand = new RelayCommand(DemoAddRandomBooks, CanOperateWithBooks);
        RemoveBookCommand = new RelayCommand(RemoveBook, CanRemoveBook);
        EditBookCommand = new RelayCommand(EditBook, CanRemoveBook);

        LibraryVisibility = Visibility.Collapsed;
    }


    #region Properties
    public string Name => Constants.BOOKS;

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
        set => SetProperty(ref _selectedBook, value);
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

    /// <summary>
    /// Command to add a new book to the library.
    /// </summary>
    public DelegateCommand AddBookCommand
    {
        get;
    }

    /// <summary>
    /// Command to add random books to the library.
    /// </summary>
    public DelegateCommand DemoAddBooksCommand
    {
        get;
    }

    /// <summary>
    /// Command to edit a book in the library.
    /// </summary>
    public RelayCommand EditBookCommand
    {
        get;
    }

    /// <summary>
    /// Command to delete a book from the library.
    /// </summary>
    public DelegateCommand RemoveBookCommand
    {
        get;
    }

    /// <summary>
    /// Determines whether operations can be performed on the books in the library.
    /// </summary>
    /// <returns>true if the library has a book list; otherwise, false.</returns>
    private bool CanOperateWithBooks()
    {
        return _bookManager.Library.Id != 0;
    }

    /// <summary>
    /// Determines whether a book can be removed from the library.
    /// </summary>
    /// <returns>true if the library has a book list and the selected book is not null; otherwise, false.</returns>
    private bool CanRemoveBook()
    {
        return 0 < _bookManager.Library.TotalBooks && SelectedBook is Book;
    }
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
    private void RemoveBook()
    {
        var deletedBookId = SelectedBook?.Id;

        if (_bookManager.TryRemoveBook(SelectedBook))
            MessageHandler.SendToStatusBar($"From library ID: {BookManager.Library.Id} was deleted book with ID: {deletedBookId}");
    }

    /// <summary>
    /// Sorts the books in the library.
    /// </summary>
    private void SortBooks()
    {
        _bookManager.SortBooks();
        MessageHandler.SendToStatusBar($"Library ID:{BookManager.Library.Id} was sorted");
    }
    #endregion


    #region Private Members
    private readonly IBookManageable _bookManager;
    private bool _isChecked;
    private bool _isEnabled = true;
    private Visibility _libraryVisibility;
    #endregion
}
