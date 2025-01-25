using System.Windows;
using BookLibraryManager.Common;
using BookLibraryManager.DemoApp.Model;
using BookLibraryManager.DemoApp.Util;
using BookLibraryManager.TestApp.View;
using Microsoft.Win32;

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
    public AddBookViewModel(ILibrary libraryManager, out Book book)
    {
        _libraryManager = libraryManager;

        Book =
            new()
            {
                Id = 1,
                Author = "Author",
                Title = "Title",
                TotalPages = 1,
                PublishDate = 1970,
                Description = "Short description"
            };

        _originalBook =
            new()
            {
                Id = Book.Id,
                Author = Book.Author,
                Title = Book.Title,
                TotalPages = Book.TotalPages,
                PublishDate = Book.PublishDate,
                Content = Book.Content,
                ContentType = Book.ContentType,
                Description = Book.Description
            };

        LoadingState = "Load content";
        ExecuteButtonName = "Add Book";
        LoadContentCommand = new RelayCommand(LoadContent, CanLoadContent);
        ExecuteCommand = new DelegateCommand<Window>(AddBook);
        CancelCommand = new DelegateCommand<Window>(CancelAddBook);

        WindowTitle = "Add Book";
        _addBookWindow = new ActionWithBookWindow() { DataContext = this };
        _addBookWindow.ShowDialog();

        book = Book;
    }


    #region Properties
    /// <summary>
    /// The book being added.
    /// </summary>
    public Book Book
    {
        get => _book;
        private set => SetProperty(ref _book, value);
    }

    public string LoadingState
    {
        get; private set;
    }

    /// <summary>
    /// Title of the AddBook window.
    /// </summary>
    public string WindowTitle
    {
        get;
    }

    /// <summary>
    /// Value indicating whether a new book should be added to the library at the end of the procedure.
    /// </summary>
    public bool CanAddBook
    {
        get; private set;
    }

    /// <summary>
    /// Name of the Execute button on the ExecuteCancelPanelView.
    /// </summary>
    public string ExecuteButtonName
    {
        get;
    }
    #endregion

    #region Commands
    public DelegateCommand LoadContentCommand
    {
        get;
    }

    /// <summary>
    /// Command to add a book to the library.
    /// </summary>
    public DelegateCommand<Window> ExecuteCommand
    {
        get;
    }

    /// <summary>
    /// Command to cancel adding a book.
    /// </summary>
    public DelegateCommand<Window> CancelCommand
    {
        get;
    }
    #endregion

    #region Methods
    private void LoadContent()
    {
        var loader = new Loader();

        // TEST
        //var filePath = GetPathToXmlFileLibrary();

        //newLib = new LibraryBookManagerModel();
        //Task.Run(async () => await loader.LoadDataAsync(() => newLib.LoadLibrary(new XmlBookListLoader(), filePath)));
        MessageBox.Show("Haven't done yet!");
    }

    // TEST
    private bool CanLoadContent() => newLib?.ActionFinished ?? true;

    // TEST
    ILibrary newLib;

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
    /// Cancels adding the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CancelAddBook(Window window)
    {
        CanAddBook = false;
        Book.Id = _originalBook.Id;
        Book.Author = _originalBook.Author;
        Book.Title = _originalBook.Title;
        Book.TotalPages = _originalBook.TotalPages;
        CloseWindow(window);
    }

    /// <summary>
    /// Closes the specified window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CloseWindow(Window window)
    {
        window?.Close();
        _addBookWindow?.Close();
    }

    /// <summary>
    /// Returns the path to the XML file with the library.
    /// </summary>
    private string? GetPathToXmlFileLibrary()
    {
        var openDialog = new OpenFileDialog()
        {
            Title = "Load the library",
            DefaultExt = ".xml",
            Filter = "XML Books (.xml)|*.xml"
        };
        var dialogResult = openDialog.ShowDialog();
        if (!dialogResult.HasValue || !dialogResult.Value)
            return null;
        var path = openDialog.FileName;

        return path;
    }
    #endregion

    #region Fields
    private readonly ILibrary _libraryManager;
    private readonly ActionWithBookWindow _addBookWindow;
    private readonly Book _originalBook;
    private Book _book;
    #endregion
}
