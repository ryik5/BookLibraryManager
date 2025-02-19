using System.Windows;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;
using LibraryManager.Views;

namespace LibraryManager.ViewModels;

/// <summary>
/// CreatorBookDetailsViewModel for working with a book.
/// </summary>
/// <author>YR 2025-01-09</author>
internal class CreatorBookDetailsViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the CreatorBookDetailsViewModel class.
    /// </summary>
    public CreatorBookDetailsViewModel(IBookManageable libraryManager)
    {
        _libraryManager = libraryManager;

        LoadingState = Constants.LOAD_CONTENT;
        LoadBookContentCommand = new RelayCommand(async () => await LockButtonsOnExecuteAsync(LoadBookContent));
        ClearBookContentCommand = new RelayCommand(ClearBookContent);
        SaveContentCommand = new RelayCommand(async () => await LockButtonsOnExecuteAsync(SaveContent));
        IsLoadEnabled = true;
        IsSaveEnabled = false;
        NoButtonVisibility = Visibility.Collapsed;
        CancelButtonVisibility = Visibility.Visible;
    }


    #region Properties
    /// <summary>
    /// The selected book 
    /// </summary>
    public Book Book
    {
        get => _book;
        set => SetProperty(ref _book, value);
    }

    /// <summary>
    /// Gets or sets text of the current loading state.
    /// </summary>
    /// <value>The loading state.</value>
    public string LoadingState
    {
        get => _loadingState;
        set => SetProperty(ref _loadingState, value);
    }

    /// <summary>
    /// Title of the BookDetails window.
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

    public Visibility NoButtonVisibility
    {
        get => _noButtonVisibility;
        set => SetProperty(ref _noButtonVisibility, value);
    }

    public Visibility CancelButtonVisibility
    {
        get => _cancelButtonVisibility;
        set => SetProperty(ref _cancelButtonVisibility, value);
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
    /// Event raised when an action is started and finished.
    /// </summary>
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
    /// <summary>
    /// Locks the buttons while executing the specified asynchronous function.
    /// </summary>
    /// <param name="func">The asynchronous function to execute.</param>
    private async Task LockButtonsOnExecuteAsync(Func<Task> func)
    {
        IsUnLocked = false;
        await func();
        IsUnLocked = true;
    }

    /// <summary>
    /// Adds a specified number of example books to the library.
    /// </summary>
    /// <param name="countBooks">The number of example books to add.</param>
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
        WindowTitle = Constants.ADD_BOOK;
        ExecuteButtonName = Constants.ADD_BOOK;
        ExecuteCommand = new DelegateCommand<Window>(AddBook);
        CancelCommand = new DelegateCommand<Window>(CancelAddBook);
        // Create and show the dialog window
        _editBookDetailsWindow = new EditBookDetailsWindow() { DataContext = this };
        _editBookDetailsWindow.ShowDialog();
    }


    /// <summary>
    /// Clears the book content and resets the loading state.
    /// </summary>
    private void ClearBookContent()
    {
        // Clear the book content
        Book.Content = null;
        LoadingState = Constants.LOAD_CONTENT;
        // Enable the load functionality and disable the save functionality
        IsLoadEnabled = true;
        IsSaveEnabled = false;
    }

    /// <summary>
    /// Loads the book content asynchronously.
    /// </summary>
    private async Task LoadBookContent()
    {
        // Disable the save functionality
        IsSaveEnabled = false;

        // Create a new loader instance
        var loader = new Handler();

        // Subscribe to the loading finished event
        ActionFinished += NewLib_LoadingFinished;

        MessageHandler.PublishDebugMessage(Constants.LOAD_CONTENT);

        // Load the book content (TODO: select type of content to load)
        var taskResult = await Handler.TryExecuteTaskAsync(() => OpenContentAttachDialog(Book));

        await Task.Yield();
        var isNotLoaded = (Book.Content is null) || taskResult.IsFaulted || taskResult.IsCanceled;
        // Set the loading state message
        var msg = isNotLoaded ? Constants.LOAD_CONTENT : Constants.CONTENT_WAS_LOADED;

        // Invoke the loading finished event
        ActionFinished?.Invoke(this, new ActionFinishedEventArgs { Message = msg, IsFinished = true });

        // Yield to allow other tasks to run before next

        IsSaveEnabled = !isNotLoaded;

        if (isNotLoaded)
            MessageHandler.PublishDebugMessage(Constants.CONTENT_WAS_NOT_LOADED_SUCCESSFULLY);
        else
            MessageHandler.PublishDebugMessage($"{Constants.NEW_CONTENT_WAS_LOADED_INTO_BOOK} '{Book.Content?.OriginalPath}'");

        // Unsubscribe from the loading finished event
        ActionFinished -= NewLib_LoadingFinished;
    }

    /// <summary>
    /// Saves the book content (not implemented yet).
    /// </summary>
    private async Task SaveContent()
    {
        if (Book.Content != null)
        {
            // Subscribe to the saving finished event
            ActionFinished += NewLib_LoadingFinished;

            MessageHandler.PublishDebugMessage($"{Constants.ATTEMPT_TO} {Constants.SAVE_CONTENT}");

            var result = await Handler.TryExecuteTaskAsync(() => SaveContentDialog(Book));

            var msg = result.Result ? Constants.CONTENT_SAVED_SUCCESSFULLY : Constants.FAILED_TO_SAVE_CONTENT;

            ActionFinished?.Invoke(this, new ActionFinishedEventArgs { Message = msg, IsFinished = true });
        }
        else
        {
            new MessageBoxHandler().Show(Constants.NO_CONTENT_TO_SAVE);
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
            Message = Constants.LOADING_STARTED,
            IsFinished = false
        });

        var selectorFiles = new SelectionDialogHandler();
        book.Content = await selectorFiles.ReadContentOpenDialogTask();
    }

    /// <summary>
    /// Saves the content of the specified book using a dialog.
    /// </summary>
    /// <param name="book">The book whose content is to be saved.</param>
    /// <returns>A boolean indicating whether the content was saved successfully.</returns>
    private async Task<bool> SaveContentDialog(Book book)
    {
        // Invoke the loading started event
        ActionFinished?.Invoke(this, new ActionFinishedEventArgs
        {
            Message = Constants.SAVING_STARTED,
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
        MessageHandler.PublishMessage($"{Constants.LAST_ADDED_BOOK} '{book.Title}' ({Constants.ID}: '{book.Id}')");
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
        MessageHandler.PublishDebugMessage(Constants.ADDING_BOOK_WAS_CANCELLED);
        CloseWindow(window);
    }

    /// <summary>
    /// Closes the specified window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CloseWindow(Window window)
    {
        window?.Close();
        _editBookDetailsWindow?.Close();
    }
    #endregion

    #region Fields
    private readonly IBookManageable _libraryManager;
    private EditBookDetailsWindow _editBookDetailsWindow;
    private Book _book;
    private Visibility _noButtonVisibility;
    private Visibility _cancelButtonVisibility;
    private string _loadingState;
    private bool _isLoadEnabled;
    private bool _isSaveEnabled;
    private bool _isUnLocked = true;
    #endregion
}
