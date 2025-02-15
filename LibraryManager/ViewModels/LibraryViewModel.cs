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
        SaveLibraryCommand = new RelayCommand(async () => await LockButtonsOnExecuteAsync(SaveLibrary), CanOperateWithBooks);
        CloseLibraryCommand = new RelayCommand(CloseLibrary, CanOperateWithBooks);
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
    public RelayCommand CloseLibraryCommand
    {
        get;
    }

    /// <summary>
    /// Determines whether operations can be performed on the books in the library.
    /// </summary>
    /// <returns>true if the library has a book list; otherwise, false.</returns>
    private bool CanOperateWithBooks()
    {
        return _libraryManager.Library.Id != 0;
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

        MessageHandler.SendToStatusBar($"Created a new library with id: {_libraryManager.Library.Id}");
        MessageHandler.SendToStatusBar("0", EInfoKind.TotalBooks);
    }

    /// <summary>
    /// Loads an existing library from a file.
    /// </summary>
    private async Task LoadLibrary()
    {
        var filePath = new SelectionDialogHandler().GetPathToXmlFile();

        MessageHandler.SendToStatusBar("Library is loading go on...", EInfoKind.CommonMessage);
        await Task.Yield();

        var result = await Handler.TryExecuteTaskAsync(() => LoadLibraryTask(filePath));

        if (result?.Result ?? false)
        {
            MessageHandler.SendDebugMessag($"The library was loaded from the path: '{filePath}'");
            MessageHandler.SendToStatusBar($"{_libraryManager.Library?.TotalBooks}", EInfoKind.TotalBooks);
            MessageHandler.SendToStatusBar($"Library loaded with ID: {_libraryManager.Library.Id}");
        }
        else
        {
            MessageHandler.SendDebugMessag($"Library was not loaded from the path: '{filePath}'");
        }

        MessageHandler.SendDebugMessag("Library loading finished.");
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
            var selectedFolder = new SelectionDialogHandler().GetPathToFolder("Save books dialog");
            if (string.IsNullOrEmpty(selectedFolder))
                throw new Exception("Folder wasn't selected");

            var pathToFile = Path.Combine(selectedFolder, $"{_libraryManager.Library.Id}.xml");

            var file = new FileInfo(pathToFile);
            if (file.Exists)
                file.Delete();

            MessageHandler.SendToStatusBar("Library is saving go on...", EInfoKind.CommonMessage);
            await Task.Yield();

            var result = await Handler.TryExecuteTaskAsync(() => SaveLibraryTask(pathToFile));

            var text = result?.Result ?? false
                    ? $"Saved Library with id:{_libraryManager.Library.Id}. Total books:{_libraryManager.Library?.TotalBooks}. Library's path:{pathToFile}"
            : "Library wasn't saved";
            MessageHandler.SendToStatusBar(text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
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
            MessageHandler.SendToStatusBar($"Library '{_libraryManager.Library.Id}' was closed");

            _libraryManager.TryCloseLibrary();
            UpdateLibraryState();
            MessageHandler.SendDebugMessag("Library updating");
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
        MessageHandler.SendToStatusBar($"{e?.TotalBooks ?? 0}", EInfoKind.TotalBooks);
    }
    #endregion


    #region Private Members
    private readonly ILibraryManageable _libraryManager;
    private bool _isChecked;
    private bool _isEnabled = true;
    private bool _isUnLocked = true;
    #endregion
}
