using System.Windows;
using BookLibraryManager.Common;
using BookLibraryManager.DemoApp.Events;
using BookLibraryManager.DemoApp.Model;
using BookLibraryManager.DemoApp.Util;
using BookLibraryManager.DemoApp.ViewModel;
using BookLibraryManager.TestApp.View;

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
    public AddBookViewModel(ILibrary libraryManager)
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

        ExecuteButtonName = "Add Book";

        LoadingState = "Load content";
        canDoLoading = true;
        LoadBookContentCommand = new RelayCommand(LoadBookContent, CanLoadContent);
        ClearBookContentCommand = new RelayCommand(ClearBookContent, CanClearContent);

        ExecuteCommand = new DelegateCommand<Window>(AddBook);
        CancelCommand = new DelegateCommand<Window>(CancelAddBook);

        WindowTitle = "Add Book";
        _addBookWindow = new ActionWithBookWindow() { DataContext = this };
        _addBookWindow.ShowDialog();
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
        get => _loadingState;
        private set => SetProperty(ref _loadingState, value);
    }
    private string _loadingState;

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
    #endregion

    #region Commands
    public DelegateCommand LoadBookContentCommand
    {
        get;
    }

    public DelegateCommand ClearBookContentCommand
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
    private void ClearBookContent()
    {
        Book.Content = null;
        LoadingState = "Load content";
    }

    private async void LoadBookContent()
    {
        var loader = new Loader();

        LoadingFinished += NewLib_LoadingFinished;
        // TODO 1st step - select type of content to load
        Book.Content = await loader.LoadDataAsync<MediaData>(() => OpenBitmapImage());

        var msg = (Book.Content is null) ? "Load content" : "Content was loaded";

        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = msg, IsFinished = true });

        await Task.Yield();

        LoadingFinished -= NewLib_LoadingFinished;
    }

    private void NewLib_LoadingFinished(object? sender, ActionFinishedEventArgs e)
    {
        LoadingState = e.Message;
        canDoLoading = e.IsFinished;
    }

    private MediaData? OpenBitmapImage()
    {
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = "Loading started", IsFinished = false });

        var selectorFiles = new SelectionDialogHandler();

        return selectorFiles.SelectMediaData();
    }

    public event EventHandler<ActionFinishedEventArgs> LoadingFinished;

    private bool CanLoadContent() => canDoLoading;

    private bool CanClearContent() => canDoLoading && Book?.Content != null;

    private bool canDoLoading
    {
        get; set;
    }

    /// <summary>
    /// Adds the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void AddBook(Window window)
    {
        _libraryManager.AddBook(Book);
        MessageHandler.SendToStatusBar(StatusBarKindEnum.MainWindow, $"Last added book: '{Book.Title}'");
        CloseWindow(window);
    }

    /// <summary>
    /// Cancels adding the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CancelAddBook(Window window)
    {
        Book = null;
        MessageHandler.SendToStatusBar(StatusBarKindEnum.MainWindow, "Adding book was canceled");
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
    #endregion

    #region Fields
    private readonly ILibrary _libraryManager;
    private readonly ActionWithBookWindow _addBookWindow;
    private Book _book;
    #endregion
}
