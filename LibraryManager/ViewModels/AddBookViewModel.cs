using System.Windows;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;
using LibraryManager.Views;

namespace LibraryManager.ViewModels;

/// <summary>
/// ViewModel for adding a new book.
/// </summary>
/// <author>YR 2025-01-09</author>
internal class AddBookViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the AddBookViewModel class.
    /// </summary>
    /// <param name="book">An example of the book to be added.</param>
    public AddBookViewModel(ILibrary libraryManager)
    {
        _libraryManager = libraryManager;

        LoadingState = "Load content";
        LoadBookContentCommand = new RelayCommand(LoadBookContent);
        ClearBookContentCommand = new RelayCommand(ClearBookContent);
        SaveContentCommand = new RelayCommand(SaveContent);
        IsEnableLoad = true;
    }


    #region Properties
    /// <summary>
    /// The book being added.
    /// </summary>
    public Book Book
    {
        get => _book;
        set => SetProperty(ref _book, value);
    }

    public string LoadingState
    {
        get => _loadingState;
        set => SetProperty(ref _loadingState, value);
    }
    private string _loadingState;

    /// <summary>
    /// Title of the AddBook window.
    /// </summary>
    public string WindowTitle
    {
        get; set;
    }

    /// <summary>
    /// Name of the Execute button on the ExecuteCancelPanelView.
    /// </summary>
    public string ExecuteButtonName
    {
        get; set;
    }
    #endregion

    #region Commands
    public DelegateCommand LoadBookContentCommand
    {
        get; set;
    }

    public DelegateCommand ClearBookContentCommand
    {
        get; set;
    }

    public RelayCommand SaveContentCommand
    {
        get; set;
    }

    /// <summary>
    /// Command to add a book to the library.
    /// </summary>
    public DelegateCommand<Window> ExecuteCommand
    {
        get; set;
    }

    /// <summary>
    /// Command to cancel adding a book.
    /// </summary>
    public DelegateCommand<Window> CancelCommand
    {
        get; set;
    }

    public bool IsEnableLoad
    {
        get => _isEnableLoad;
        set
        {
            if (SetProperty(ref _isEnableLoad, value))
                IsEnableSave = !value;
        }
    }
    private bool _isEnableLoad;

    public bool IsEnableSave
    {
        get => _isEnableSave;
        set => SetProperty(ref _isEnableSave, value);
    }
    private bool _isEnableSave;

    public event EventHandler<ActionFinishedEventArgs> LoadingFinished;
    #endregion

    #region Methods
    public virtual void ShowDialog()
    {
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

        WindowTitle = "Add Book";
        ExecuteButtonName = "Add Book";
        ExecuteCommand = new DelegateCommand<Window>(AddBook);
        CancelCommand = new DelegateCommand<Window>(CancelAddBook);

        _addBookWindow = new ActionWithBookWindow() { DataContext = this };
        _addBookWindow.ShowDialog();
    }

    private void ClearBookContent()
    {
        Book.Content = null;
        LoadingState = "Load content";
        IsEnableLoad = true;
    }

    private async void LoadBookContent()
    {
        _isEnableLoad = false;
        _isEnableSave = false;
        RaisePropertyChanged(nameof(IsEnableLoad));
        RaisePropertyChanged(nameof(IsEnableSave));

        var loader = new Loader();

        LoadingFinished += NewLib_LoadingFinished;
        // TODO 1st step - select type of content to load
        Book.Content = await loader.LoadDataAsync<MediaData>(() => OpenBitmapImage());

        var msg = (Book.Content is null) ? "Load content" : "Content was loaded";

        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = msg, IsFinished = true });

        await Task.Yield();

        IsEnableSave = Book.Content is null ? false : true;
        LoadingFinished -= NewLib_LoadingFinished;
    }

    private void SaveContent()
    {
        throw new NotImplementedException();
    }

    private MediaData? OpenBitmapImage()
    {
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = "Loading started", IsFinished = false });

        var selectorFiles = new SelectionDialogHandler();

        return selectorFiles.SelectMediaData();
    }

    private void NewLib_LoadingFinished(object? sender, ActionFinishedEventArgs e)
    {
        LoadingState = e.Message;
        IsEnableLoad = e.IsFinished;
    }

    /// <summary>
    /// Adds the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void AddBook(Window window)
    {
        _libraryManager.AddBook(Book);
        MessageHandler.SendToStatusBar(EWindowKind.MainWindow, $"Last added book: '{Book.Title}'");
        CloseWindow(window);
    }

    /// <summary>
    /// Cancels adding the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CancelAddBook(Window window)
    {
        Book = null;
        MessageHandler.SendToStatusBar(EWindowKind.MainWindow, "Adding book was canceled");
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
    private ActionWithBookWindow _addBookWindow;
    private Book _book;
    #endregion
}
