using System.IO;
using System.Text;
using System.Windows;
using AppBookManager;
using BookLibraryManager.Common;
using BookLibraryManager.DemoApp.Events;
using BookLibraryManager.DemoApp.Model;
using Microsoft.Win32;

namespace BookLibraryManager.TestApp.ViewModel;
/// <summary>
/// ViewModel for the main view of the Book Library Manager application.
/// </summary>
/// <author>YR 2025-01-14</author>
public class MainViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainViewModel"/> class.
    /// </summary>
    public MainViewModel()
    {
        _libraryManager = new LibraryBookManagerModel();
        _statusBarKind = StatusBarKindEnum.MainWindow;
        StatusBarItems = new StatusBarModel(_statusBarKind);

        CreateNewCommand = new RelayCommand(CreateNewLibrary);
        LoadLibraryCommand = new RelayCommand(LoadLibrary);

        SaveLibraryCommand = new RelayCommand(SaveLibrary, CanOperateWithBooks);
        SortLibraryCommand = new RelayCommand(SortLibrary, CanOperateWithBooks);

        AddBookCommand = new RelayCommand(AddBook, CanOperateWithBooks);
        AddRandomBooksCommand = new RelayCommand(AddRandomBooks, CanOperateWithBooks);
        RemoveBookCommand = new RelayCommand(RemoveBook, CanRemoveBook);
        FindBookCommand = new RelayCommand(FindBook, CanOperateWithBooks);
        ToggleViewCommand = new RelayCommand(ToggleView);

        ExitApplicationCommand = new DelegateCommand<Window>(window => Application.Current.Shutdown());

        LibraryViewHeight = new GridLength(1, GridUnitType.Star);
        LogViewHeight = new GridLength(0);
        ViewName = "Toggle to Log";
    }

    #region Commands
    /// <summary>
    /// Command to create a new library.
    /// </summary>
    public DelegateCommand CreateNewCommand
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
    /// Command to sort the books in the library.
    /// </summary>
    public DelegateCommand SortLibraryCommand
    {
        get;
    }

    /// <summary>
    /// Command to add a new book to the library.
    /// </summary>
    public DelegateCommand AddBookCommand
    {
        get;
    }

    /// <summary>
    /// Command to add random books to the library.
    /// </summary>
    public DelegateCommand AddRandomBooksCommand
    {
        get;
    }

    /// <summary>
    /// Command to delete a book from the library.
    /// </summary>
    public DelegateCommand RemoveBookCommand
    {
        get;
    }

    /// <summary>
    /// Command to find books in the library.
    /// </summary>
    public DelegateCommand FindBookCommand
    {
        get;
    }

    /// <summary>
    /// Command to toggle the view between library and log.
    /// </summary>
    public DelegateCommand ToggleViewCommand
    {
        get;
    }

    /// <summary>
    /// Command to exit the application.
    /// </summary>
    public DelegateCommand<Window> ExitApplicationCommand
    {
        get;
    }
    #endregion

    #region Properties
    public StatusBarModel StatusBarItems
    {
        get;
    }

    /// <summary>
    /// Gets or sets the text log for displaying logging messages.
    /// </summary>
    public string ViewName
    {
        get => _viewName;
        set => SetProperty(ref _viewName, value);
    }
    private string _viewName;

    /// <summary>
    /// Gets or sets the text log for displaying logging messages.
    /// </summary>
    public string TextLog
    {
        get => _textLog;
        set => SetProperty(ref _textLog, value);
    }
    private string _textLog = string.Empty;

    public ILibrary Library => _libraryManager;

    /// <summary>
    /// Gets or sets the height of the log view for displaying logging messages.
    /// </summary>
    public GridLength LogViewHeight
    {
        get => _logViewHeight;
        set => SetProperty(ref _logViewHeight, value);
    }
    private GridLength _logViewHeight;

    /// <summary>
    /// Gets or sets the height of the library view.
    /// </summary>
    public GridLength LibraryViewHeight
    {
        get => _libraryViewHeight;
        set => SetProperty(ref _libraryViewHeight, value);
    }
    private GridLength _libraryViewHeight;
    #endregion

    #region Methods
    /// <summary>
    /// Determines whether operations can be performed on the books in the library.
    /// </summary>
    /// <returns>true if the library has a book list; otherwise, false.</returns>
    private bool CanOperateWithBooks()
    {
        return _libraryManager?.BookList != null;
    }

    /// <summary>
    /// Determines whether a book can be removed from the library.
    /// </summary>
    /// <returns>true if the library has a book list and the selected book is not null; otherwise, false.</returns>
    private bool CanRemoveBook()
    {
        return _libraryManager?.BookList != null && _libraryManager?.SelectedBook is Book;
    }

    /// <summary>
    /// Toggles the visibility between the library view and the log view.
    /// </summary>
    /// <remarks>
    /// This method switches the heights of the library view and the log view.
    /// If the library view is currently visible, it hides the library view and shows the log view.
    /// If the log view is currently visible, it hides the log view and shows the library view.
    /// The ViewName property is updated to reflect the current state.
    /// </remarks>
    private void ToggleView()
    {
        var nameView = string.Empty;
        if (LibraryViewHeight.Value > 0)
        {
            LibraryViewHeight = new GridLength(0);
            LogViewHeight = new GridLength(1, GridUnitType.Star);
            ViewName = "Toggle to Library";
            nameView = "Log";
        }
        else
        {
            LibraryViewHeight = new GridLength(1, GridUnitType.Star);
            LogViewHeight = new GridLength(0);
            ViewName = "Toggle to Log";
            nameView = "Library";
        }

        SendMessageToStatusBar($"View was switched to {nameView}");
    }

    /// <summary>
    /// Adds a new book to the library.
    /// </summary>
    private void AddBook()
    {
        var canAddBook = new AddBookViewModel(out var book).CanAddBook;
        if (canAddBook)
        {
            _libraryManager.AddBook(book);
            TextLog += $"Last added book id:{book.Id}\nTotal books:{_libraryManager?.NumberOfBooks}";

            SendMessageToStatusBar($"Last added book: '{book.Title}'");
        }
        else
        {
            TextLog += $"Adding book was canceled";
        }
    }

    /// <summary>
    /// Adds randomly filled books to the library.
    /// </summary>
    private void AddRandomBooks()
    {
        counterUsingAddRandomBooks++;

        for (var i = 0; i < 10; i++)
        {
            var testBook = new Book()
            {
                Id = Random.Shared.Next(),
                Author = $"{RepeaterWords(tenWords[tenWords.Length - 1 - i], counterUsingAddRandomBooks)}",
                Title = $"{RepeaterWords(tenWords[i], counterUsingAddRandomBooks)} {Random.Shared.Next()}",
                TotalPages = 20,
                PublishDate = 2020
            };

            _libraryManager.AddBook(testBook);
        }
        var text = $"Added 10 random named books\ntotal books in library: {_libraryManager?.NumberOfBooks}";
        TextLog += text;

        SendMessageToStatusBar(text);
    }

    /// <summary>
    /// Creates a new library or changes an instance of the existing library by creating a new one.
    /// </summary>
    private void CreateNewLibrary()
    {
        counterUsingAddRandomBooks = 0;

        _libraryManager.CreateNewLibrary(Random.Shared.Next());
        var text = $"Created a new library with id: {_libraryManager.Id}";
        TextLog += text;

        SendMessageToStatusBar(text);
    }

    /// <summary>
    /// Loads an existing library from a file.
    /// </summary>
    private void LoadLibrary()
    {
        counterUsingAddRandomBooks = 0;

        var filePath = GetPathToXmlFileLibrary();

        if (_libraryManager.LoadLibrary(new XmlBookListLoader(), filePath))
        {
            var text = $"Library was loaded with id:{_libraryManager.Id}. Total books:{_libraryManager?.NumberOfBooks}. Library's path: {filePath}";
            TextLog += text;

            SendMessageToStatusBar(text);
        }
        else
        {
            MessageBox.Show("Library was not loaded");
        }
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

    /// <summary>
    /// Deletes a book from the library.
    /// </summary>
    private void RemoveBook()
    {
        TextLog += "\n----";

        var deletedBookId = _libraryManager.SelectedBook?.Id;
        var result = _libraryManager.RemoveBook(_libraryManager.SelectedBook);
        var text = result
            ? $"Deleted book with id: {deletedBookId}"
            : "Nothing to delete";
        TextLog += text;

        SendMessageToStatusBar(text);
    }

    /// <summary>
    /// Saves the current library to a file.
    /// </summary>
    private void SaveLibrary()
    {
        try
        {
            var selectedFolder = GetPathToFolderToStoreLibrary();

            var pathToFile = Path.Combine(selectedFolder, $"{_libraryManager.Id}.xml");
            var file = new FileInfo(pathToFile);
            if (file.Exists)
                file.Delete();

            var result = _libraryManager.SaveLibrary(new XmlBookListSaver(), pathToFile);

            var text = result
                    ? $"Saved Library with id:{_libraryManager.Id}. Total books:{_libraryManager?.NumberOfBooks}. Library's path:{pathToFile}"
            : "Library wasn't saved";
            TextLog += text;
            SendMessageToStatusBar(text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// Returns the path to the folder to store the library.
    /// </summary>
    private string GetPathToFolderToStoreLibrary()
    {
        var openDialog = new OpenFolderDialog()
        {
            Title = "Save books dialog",
            Multiselect = false,
            ValidateNames = true
        };
        var dialogResult = openDialog.ShowDialog();
        if (!dialogResult.HasValue || !dialogResult.Value)
            throw new Exception("No selected folder to save!");
        return openDialog.FolderName;
    }

    /// <summary>
    /// Sorts the books in the library.
    /// </summary>
    private void SortLibrary()
    {
        _libraryManager.SortLibrary();
        SendMessageToStatusBar("Library was sorted");
    }

    /// <summary>
    /// Finds books in the library that contain the specified title part.
    /// </summary>
    private void FindBook()
    {
        SendMessageToStatusBar("Open 'Find books window'");

        _ = new FindBookViewModel(_libraryManager);
    }

    /// <summary>
    /// Repeats the word by the specified number of times.
    /// </summary>
    private string RepeaterWords(string word, int times)
    {
        StringBuilder stringBuilder = new();
        for (var i = 0; i < times; i++)
        {
            stringBuilder.Append($"{word}");
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Sends a message to the status bar.
    /// </summary>
    /// <param name="msg">The message to send.</param>
    private void SendMessageToStatusBar(string msg)
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Publish(new StatusBarEventArgs
        {
            Message = msg,
            StatusBarKind = _statusBarKind
        });
    }
    #endregion

    #region Private Members
    private int counterUsingAddRandomBooks = 0;
    private readonly string[] tenWords = ["a", "A", "b", "B", "c", "C", "e", "E", "f", "F"];
    private readonly LibraryBookManagerModel _libraryManager;
    private readonly StatusBarKindEnum _statusBarKind;
    #endregion
}
