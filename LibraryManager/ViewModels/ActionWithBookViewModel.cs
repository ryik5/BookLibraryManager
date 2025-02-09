using System.Windows;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;
using LibraryManager.Views;

namespace LibraryManager.ViewModels;

/// <summary>
/// ActionWithBookViewModel for working with a book.
/// </summary>
/// <author>YR 2025-01-09</author>
internal class ActionWithBookViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the ActionWithBookViewModel class.
    /// </summary>
    /// <param name="book">An example of the book to be managed.</param>
    public ActionWithBookViewModel(IBookManageable libraryManager)
    {
        _libraryManager = libraryManager;

        LoadingState = "Load content";
        LoadBookContentCommand = new RelayCommand(LoadBookContent);
        ClearBookContentCommand = new RelayCommand(ClearBookContent);
        SaveContentCommand = new RelayCommand(SaveContent);
        IsLoadEnabled = true;
        IsSaveEnabled = false;
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

    public event EventHandler<ActionFinishedEventArgs> LoadingFinished;
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

    /// <summary>
    /// Gets or sets a value indicating whether the load content is enabled.
    /// </summary>
    public bool IsLoadEnabled
    {
        get => _isLoadEnabled;
        set => SetProperty(ref _isLoadEnabled, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the loaded content is enabled to save on the disk.
    /// </summary>
    public bool IsSaveEnabled
    {
        get => _isSaveEnabled;
        set => SetProperty(ref _isSaveEnabled, value);
    }
    #endregion

    #region Methods
    public void AddExampleBooks(int countBooks)
    {
        for (var i = 0; i < countBooks; i++)
            AddBook(DemoBookMaker.GenerateBook());
    }

    /// <summary>
    /// Displays the dialog for adding a new book.
    /// </summary>
    public virtual void ShowDialog()
    {
        // Generate a new book with default values
        Book = DemoBookMaker.GenerateBook();

        // Set up the dialog window properties
        WindowTitle = "Add Book";
        ExecuteButtonName = "Add Book";
        ExecuteCommand = new DelegateCommand<Window>(AddBook);
        CancelCommand = new DelegateCommand<Window>(CancelAddBook);
        // Create and show the dialog window
        _addBookWindow = new ActionWithBookWindow() { DataContext = this };
        _addBookWindow.ShowDialog();
    }


    /// <summary>
    /// Clears the book content and resets the loading state.
    /// </summary>
    private void ClearBookContent()
    {
        // Clear the book content
        Book.Content = null;
        LoadingState = "Load content";
        // Enable the load functionality and disable the save functionality
        IsLoadEnabled = true;
        IsSaveEnabled = false;
    }

    /// <summary>
    /// Loads the book content asynchronously.
    /// </summary>
    private async void LoadBookContent()
    {
        // Disable the save functionality
        IsSaveEnabled = false;

        // Create a new loader instance
        var loader = new Loader();

        // Subscribe to the loading finished event
        LoadingFinished += NewLib_LoadingFinished;

        MessageHandler.SendToStatusBar($"Attempt to load new content", EInfoKind.DebugMessage);

        // Load the book content (TODO: select type of content to load)
        Book.Content = await loader.LoadDataAsync<MediaData>(() => OpenBitmapImage());

        // Set the loading state message
        var msg = (Book.Content is null) ? "Load content" : "Content was loaded";

        // Invoke the loading finished event
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = msg, IsFinished = true });

        // Yield to allow other tasks to run before next
        await Task.Yield();

        IsSaveEnabled = Book.Content is null ? false : true;

        if (Book.Content is null)
            MessageHandler.SendToStatusBar("Content was not loaded successfully", EInfoKind.DebugMessage);
        else
            MessageHandler.SendToStatusBar($"Loaded new content into the book '{Book.Content.OriginalPath}'", EInfoKind.DebugMessage);

        // Unsubscribe from the loading finished event
        LoadingFinished -= NewLib_LoadingFinished;
    }

    /// <summary>
    /// Saves the book content (not implemented yet).
    /// </summary>
    private void SaveContent()
    {
        MessageBox.Show("This functionality has not been implemented yet!");
        //  MessageHandler.SendToStatusBar($"The book '{_originalBook.Title}' (ID {_originalBook.Id}') was loaded for editing", EInfoKind.DebugMessage);

    }

    /// <summary>
    /// Opens a bitmap image (not implemented yet).
    /// </summary>
    /// <returns>The selected media data.</returns>
    private MediaData? OpenBitmapImage()
    {
        // Invoke the loading started event
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs
        {
            Message = "Loading started",
            IsFinished = false
        });

        var selectorFiles = new SelectionDialogHandler();

        return selectorFiles.SelectMediaData();
    }

    /// <summary>
    /// Handles the loading finished event.
    /// </summary>
    private void NewLib_LoadingFinished(object? sender, ActionFinishedEventArgs e)
    {
        LoadingState = e.Message;
        IsLoadEnabled = e.IsFinished;
    }

    /// <summary>
    /// Adds the book to the library 
    /// </summary>
    private void AddBook(Book book)
    {
        _libraryManager.AddBook(book);
        MessageHandler.SendToStatusBar($"Last added book '{book.Title}' (ID '{book.Id}')");
    }

    /// <summary>
    /// Adds the book to the library and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void AddBook(Window window)
    {
        AddBook(Book);

        CloseWindow(window);
    }

    /// <summary>
    /// Cancels adding the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CancelAddBook(Window window)
    {
        Book = null;
        MessageHandler.SendToStatusBar("Adding book was canceled", EInfoKind.DebugMessage);
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
    private readonly IBookManageable _libraryManager;
    private ActionWithBookWindow _addBookWindow;
    private Book _book;
    private string _loadingState;
    private bool _isLoadEnabled;
    private bool _isSaveEnabled;
    #endregion
}
