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
    public AddBookViewModel(out Book book)
    {
        Book = new Book() { Id = 1, Author = "Author", Title = "Title", PageNumber = 1 };

        _originalBook = new() { Id = Book.Id, Author = Book.Author, Title = Book.Title, PageNumber = Book.PageNumber };

        ExecuteButtonName = "Add Book";
        ExecuteCommand = new RelayCommand<Window>(AddBook);
        CancelCommand = new RelayCommand<Window>(CancelAddBook);

        WindowTitle = "Add Book";
        _addBookWindow = new ActionWithBookWindow() { DataContext = this };
        _addBookWindow.ShowDialog();

        book = Book;
    }

    /// <summary>
    /// Value indicating whether a new book should be added to the library at the end of procedure.
    /// </summary>
    public bool CanAddBook
    {
        get; private set;
    }

    /// <summary>
    /// The book being added.
    /// </summary>
    public Book Book
    {
        get => _book;
        set => SetProperty(ref _book, value);
    }
    private Book _book;

    /// <summary>
    /// Command to add a book to the library.
    /// </summary>
    public RelayCommand<Window> ExecuteCommand
    {
        get;
    }

    /// <summary>
    /// Adds the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void AddBook(Window window)
    {
        CanAddBook = true;
        CloseWindow(window);
    }

    /// <summary>
    /// Command to cancel of adding a book.
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
        CanAddBook = false;
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

    /// <summary>
    /// Title of the AddBook window.
    /// </summary>
    public string WindowTitle
    {
        get;
    }

    /// <summary>
    /// Name of the Execute button on the ExecuteCancelPanelView.
    /// </summary>
    public string ExecuteButtonName
    {
        get;
    }


    private ActionWithBookWindow _addBookWindow;
    private Book _originalBook;
}
