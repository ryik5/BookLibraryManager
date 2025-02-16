using System.IO;
using System.Windows;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;

namespace LibraryManager.ViewModels;

/// <summary>
/// LibraryViewModel view model for the library manager application.
/// </summary>
/// <author>YR 2025-02-02</author>
internal sealed class LibraryViewModel : BindableBase, IViewModelPageable
{
    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// </summary>
    /// <param name="libraryManager">The library manager model.</param>
    public LibraryViewModel(ILibraryManageable libraryManager)
    {
        _libraryManager = libraryManager;
        CreateLibraryCommand = new DelegateCommand(CreateLibrary);
        LoadLibraryCommand = new DelegateCommand(async () => await LockButtonsOnExecuteAsync(LoadLibrary));
        SaveLibraryCommand = new DelegateCommand(async () => await LockButtonsOnExecuteAsync(SaveLibrary));
        CloseLibraryCommand = new DelegateCommand(CloseLibrary);
        UpdateLibraryState();
        _libraryManager.TotalBooksChanged += LibraryTotalBooksChanged;
    }


    #region Properties
    public string Name => Constants.LIBRARY;

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
    /// Gets or sets a value indicating whether the buttons are unlocked.
    /// </summary>
    public bool IsUnLocked
    {
        get => _isUnLocked;
        set => SetProperty(ref _isUnLocked, value);
    }

    /// <summary>
    /// Gets the library manager model.
    /// </summary>
    public ILibrary Library => _libraryManager.Library;
    #endregion


    #region Commands
    /// <summary>
    /// Command to create a new library.
    /// </summary>
    public DelegateCommand CreateLibraryCommand
    {
        get;
    }

    /// <summary>
    /// Command to load an existing library.
    /// </summary>
    public DelegateCommand LoadLibraryCommand
    {
        get;
    }

    /// <summary>
    /// Command to save the current library.
    /// </summary>
    public DelegateCommand SaveLibraryCommand
    {
        get;
    }

    /// <summary>
    /// Command to close the current library.
    /// </summary>
    public DelegateCommand CloseLibraryCommand
    {
        get;
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
    /// Creates a new library or changes an instance of the existing library by creating a new one.
    /// </summary>
    private void CreateLibrary()
    {
        _libraryManager.CreateNewLibrary(Random.Shared.Next());

        UpdateLibraryState();

        MessageHandler.PublishMessage($"Created a new library with id: {_libraryManager.Library.Id}");
        MessageHandler.PublishTotalBooksInLibrary(0);
    }

    /// <summary>
    /// Loads an existing library from a file.
    /// </summary>
    private async Task LoadLibrary()
    {
        var filePath = new SelectionDialogHandler().GetPathToXmlFile();
        IsEnabled = false;

        MessageHandler.PublishMessage("Library is loading go on...");
        await Task.Yield();

        var result = await Handler.TryExecuteTaskAsync(() => LoadLibraryTask(filePath));

        if (result?.Result ?? false)
        {
            MessageHandler.PublishDebugMessage($"The library was loaded from the path: '{filePath}'");
            MessageHandler.PublishTotalBooksInLibrary(Library?.TotalBooks ?? 0);
            MessageHandler.PublishMessage($"Library loaded with ID: {Library?.Id}");
        }
        else
        {
            MessageHandler.PublishDebugMessage($"Library was not loaded from the path: '{filePath}'");
        }

        MessageHandler.PublishDebugMessage("Library loading finished.");
        UpdateLibraryState();
    }

    /// <summary>
    /// Loads an existing library from a file asynchronously.
    /// </summary>
    /// <param name="filePath">The path to the XML file to load.</param>
    /// <returns>A boolean indicating whether the library was loaded successfully.</returns>
    private async Task<bool> LoadLibraryTask(string filePath)
       => await Task.FromResult(_libraryManager.TryLoadLibrary(new XmlLibraryLoader(), filePath));


    /// <summary>
    /// Saves the current library to a file.
    /// </summary>
    private async Task SaveLibrary()
    {
        try
        {
            IsEnabled = false;
            var selectedFolder = new SelectionDialogHandler().GetPathToFolder("Save books dialog");
            if (string.IsNullOrEmpty(selectedFolder))
                throw new Exception("Folder wasn't selected");

            var pathToFile = Path.Combine(selectedFolder, $"{Library.Id}.xml");

            var file = new FileInfo(pathToFile);
            if (file.Exists)
                file.Delete();

            MessageHandler.PublishMessage("Library is saving go on...");
            await Task.Yield();

            var result = await Handler.TryExecuteTaskAsync(() => SaveLibraryTask(pathToFile));

            var text = result?.Result ?? false ? $"Library with id:{Library.Id} was saved as '{pathToFile}'" : "Library wasn't saved";
            MessageHandler.PublishMessage(text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        finally
        {
            IsEnabled = true;
        }
    }

    /// <summary>
    /// Saves the current library to a file asynchronously.
    /// </summary>
    /// <param name="pathToFile">The path to the XML file to save.</param>
    /// <returns>A boolean indicating whether the library was saved successfully.</returns>
    private async Task<bool> SaveLibraryTask(string pathToFile)
        => await Task.FromResult(_libraryManager.TrySaveLibrary(new XmlLibraryKeeper(), pathToFile));

    /// <summary>
    /// Closes the library and clears the book list.
    /// </summary>
    private void CloseLibrary()
    {
        if (_libraryManager != null)
        {
            MessageHandler.PublishMessage($"Library '{_libraryManager.Library.Id}' was closed");

            _libraryManager.TryCloseLibrary();
            UpdateLibraryState();
            MessageHandler.PublishDebugMessage("Library updating");
        }
    }

    /// <summary>
    /// Updates the library state by raising a property changed event for the Library property
    /// and setting the IsEnabled property based on whether the Library Id differs from the default value of 0.
    /// </summary>
    private void UpdateLibraryState()
    {
        RaisePropertyChanged(nameof(Library));

        IsEnabled = Library.Id != 0;
    }

    /// <summary>
    /// Handles the TotalBooksChanged event by sending message to the status bar with the new total number of books.
    /// </summary>
    /// <param name="e">The event arguments containing the new total number of books and a kind of the event.</param>
    private void LibraryTotalBooksChanged(object? sender, TotalBooksEventArgs e)
    {
        MessageHandler.PublishTotalBooksInLibrary(e?.TotalBooks ?? 0);
    }
    #endregion


    #region Private Members
    private readonly ILibraryManageable _libraryManager;
    private bool _isChecked;
    private bool _isEnabled = true;
    private bool _isUnLocked = true;
    #endregion
}
