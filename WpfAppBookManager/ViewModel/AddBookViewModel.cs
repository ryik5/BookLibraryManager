using System.Windows;
using BookLibraryManager.Common;
using BookLibraryManager.TestApp.View;
using GalaSoft.MvvmLight.CommandWpf;

namespace BookLibraryManager.TestApp.ViewModel;

/// <summary>
/// ViewModel for adding a new book.
/// </summary>
/// <author>YR 2025-01-09</author>
public class AddBookViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the AddBookViewModel class.
    /// </summary>
    /// <param name="book">An example of the book to be added.</param>
    public AddBookViewModel(Book book)
    {
        Book = book;
        _originalBook = new() { Id = book.Id, Author = book.Author, Title = book.Title, PageNumber = book.PageNumber };
        AddCommand = new RelayCommand<Window>(AddBook);
        CancelCommand = new RelayCommand<Window>(CancelAddBook);

        _addBookWindow = new AddBookWindow() { DataContext = this };
        _addBookWindow.ShowDialog();
    }

    /// <summary>
    /// Gets a value indicating whether a new book is being added.
    /// </summary>
    public bool IsAddNewBook
    {
        get; private set;
    }

    /// <summary>
    /// Gets or sets the book being added.
    /// </summary>
    public Book Book
    {
        get => _book;
        set => SetProperty(ref _book, value);
    }
    private Book _book;

    /// <summary>
    /// Gets the command to add a book.
    /// </summary>
    public RelayCommand<Window> AddCommand
    {
        get;
    }

    /// <summary>
    /// Adds the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void AddBook(Window window)
    {
        IsAddNewBook = true;
        CloseWindow(window);
    }

    /// <summary>
    /// Gets the command to cancel adding a book.
    /// </summary>
    public RelayCommand<Window> CancelCommand
    {
        get;
    }

    /// <summary>
    /// Cancels adding the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CancelAddBook(Window window)
    {
        IsAddNewBook = false;
        Book.Id = _originalBook.Id;
        Book.Author = _originalBook.Author;
        Book.Title = _originalBook.Title;
        Book.PageNumber = _originalBook.PageNumber;
        CloseWindow(window);
    }

    /// <summary>
    /// Closes the specified window.
    /// </summary>
    private void CloseWindow(Window window)
    {
        window?.Close();
        _addBookWindow?.Close();
    }

    private AddBookWindow _addBookWindow;
    private Book _originalBook;
}
