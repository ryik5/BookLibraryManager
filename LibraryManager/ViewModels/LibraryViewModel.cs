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
public class LibraryViewModel : BindableBase, IViewModelPageable
{
    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// </summary>
    /// <param name="libraryManager">The library manager model.</param>
    public LibraryViewModel(ILibraryManageable libraryManager)
    {
        _libraryManager = libraryManager;
        CreateLibraryCommand = new DelegateCommand(CreateLibrary);
        LoadLibraryCommand = new DelegateCommand(LoadLibrary);
        SaveLibraryCommand = new RelayCommand(SaveLibrary, CanOperateWithBooks);
        CloseLibraryCommand = new RelayCommand(CloseLibrary, CanOperateWithBooks);
        UpdateLibraryState();
        _libraryManager.TotalBooksChanged += LibraryTotalBooksChanged;
    }


    #region Properties
    public string Name => "Library";

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
    /// Creates a new library or changes an instance of the existing library by creating a new one.
    /// </summary>
    private void CreateLibrary()
    {
        _libraryManager.CreateNewLibrary(Random.Shared.Next());

        UpdateLibraryState();

        MessageHandler.SendToStatusBar($"Created a new library with id: {_libraryManager.Library.Id}");
        MessageHandler.SendToStatusBar("0", EInfoKind.TotalPages);
    }

    /// <summary>
    /// Loads an existing library from a file.
    /// </summary>
    private void LoadLibrary()
    {
        var filePath = new SelectionDialogHandler().GetPathToXmlFile();

        if (_libraryManager.TryLoadLibrary(new XmlLibraryLoader(), filePath))
        {
            MessageHandler.SendToStatusBar($"The library was loaded from the path: '{filePath}'", EInfoKind.DebugMessage);
            MessageHandler.SendToStatusBar($"{_libraryManager.Library?.TotalBooks}", EInfoKind.TotalPages);
            MessageHandler.SendToStatusBar($"Library loaded with ID: {_libraryManager.Library.Id}");
        }
        else
        {
            MessageHandler.SendToStatusBar($"Library was not loaded from the path: '{filePath}'", EInfoKind.DebugMessage);
        }

        MessageHandler.SendToStatusBar("Library loading finished.", EInfoKind.DebugMessage);
        UpdateLibraryState();
    }

    /// <summary>
    /// Saves the current library to a file.
    /// </summary>
    private void SaveLibrary()
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

            var result = _libraryManager.TrySaveLibrary(new XmlLibraryKeeper(), pathToFile);

            var text = result
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
    /// Closes the library and clears the book list.
    /// </summary>
    private void CloseLibrary()
    {
        if (_libraryManager != null)
        {
            MessageHandler.SendToStatusBar($"Library '{_libraryManager.Library.Id}' was closed");

            _libraryManager.TryCloseLibrary();
            UpdateLibraryState();
            MessageHandler.SendToStatusBar("Library updating", EInfoKind.DebugMessage);
        }
    }

    private void UpdateLibraryState() => IsEnabled = Library.Id != 0;


    /// <summary>
    /// Handles the TotalBooksChanged event by sending message to the status bar with the new total number of books.
    /// </summary>
    /// <param name="e">The event arguments containing the new total number of books and a kind of the event.</param>
    private void LibraryTotalBooksChanged(object? sender, TotalBooksEventArgs e)
    {
        MessageHandler.SendToStatusBar($"{e?.TotalBooks ?? 0}", EInfoKind.TotalPages);
    }
    #endregion


    #region Private Members
    private readonly ILibraryManageable _libraryManager;
    private bool _isChecked;
    private bool _isEnabled = true;
    #endregion
}
