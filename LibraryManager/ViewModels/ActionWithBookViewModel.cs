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

    public event EventHandler<ActionFinishedEventArgs> ActionFinished;
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
        var loader = new Handler();

        // Subscribe to the loading finished event
        ActionFinished += NewLib_LoadingFinished;

        MessageHandler.SendToStatusBar($"Attempt to load new content", EInfoKind.DebugMessage);

        // Load the book content (TODO: select type of content to load)
        var taskResult = await Handler.ExecuteTaskAsync(() => OpenContentAttachDialog(Book));

        await Task.Yield();
        var isNotLoaded = (Book.Content is null) || taskResult.IsFaulted || taskResult.IsCanceled;
        // Set the loading state message
        var msg = isNotLoaded ? "Load content" : "Content was loaded";

        // Invoke the loading finished event
        ActionFinished?.Invoke(this, new ActionFinishedEventArgs { Message = msg, IsFinished = true });

        // Yield to allow other tasks to run before next

        IsSaveEnabled = !isNotLoaded;

        if (isNotLoaded)
            MessageHandler.SendToStatusBar("Content was not loaded successfully", EInfoKind.DebugMessage);
        else
            MessageHandler.SendToStatusBar($"Loaded new content into the book '{Book.Content.OriginalPath}'", EInfoKind.DebugMessage);

        // Unsubscribe from the loading finished event
        ActionFinished -= NewLib_LoadingFinished;
    }

    /// <summary>
    /// Saves the book content (not implemented yet).
    /// </summary>
    private async void SaveContent()
    {
        if (Book.Content != null)
        {
            // Subscribe to the saving finished event
            ActionFinished += NewLib_LoadingFinished;

            MessageHandler.SendToStatusBar($"Attempt to save content", EInfoKind.DebugMessage);

            var result = await Handler.ExecuteTaskAsync(() => SaveContentDialog(Book));

            var msg = result.Result ? "Content saved" : "Content wasn't saved successfully";

            ActionFinished?.Invoke(this, new ActionFinishedEventArgs { Message = msg, IsFinished = true });
        }
        else
        {
            MessageBox.Show("No content to save.");
        }

        // Unsubscribe from the saving finished event
        ActionFinished -= NewLib_LoadingFinished;
    }

    /// <summary>
    /// Opens a bitmap image (not implemented yet).
    /// </summary>
    /// <returns>The selected media data.</returns>
    private async Task OpenContentAttachDialog(Book book)
    {
        // Invoke the loading started event
        ActionFinished?.Invoke(this, new ActionFinishedEventArgs
        {
            Message = "Loading started",
            IsFinished = false
        });

        var selectorFiles = new SelectionDialogHandler();
        book.Content = await selectorFiles.ReadContentOpenDialogTask();
    }

    private async Task<bool> SaveContentDialog(Book book)
    {
        // Invoke the loading started event
        ActionFinished?.Invoke(this, new ActionFinishedEventArgs
        {
            Message = "Saving started",
            IsFinished = false
        });

        var Saver = new SelectionDialogHandler();
        return await Saver.SaveContentDialogTask(book);
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
