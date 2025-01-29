using System.IO;
using System.Text;
using System.Windows;
using BookLibraryManager.Common;
using BookLibraryManager.DemoApp.Events;
using BookLibraryManager.DemoApp.Model;
using BookLibraryManager.DemoApp.ViewModel;

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

        _statusBarKind = EWindowKind.MainWindow;

        StatusBarItems = new StatusBarModel(_statusBarKind);

        CreateNewCommand = new RelayCommand(CreateNewLibrary);
        LoadLibraryCommand = new RelayCommand(LoadLibrary);
        SaveLibraryCommand = new RelayCommand(SaveLibrary, CanOperateWithBooks);
        SortLibraryCommand = new RelayCommand(SortLibrary, CanOperateWithBooks);
        CloseLibraryCommand = new RelayCommand(CloseLibrary, CanOperateWithBooks);

        AddBookCommand = new RelayCommand(AddBook, CanOperateWithBooks);
        AddRandomBooksCommand = new RelayCommand(AddRandomBooks, CanOperateWithBooks);
        RemoveBookCommand = new RelayCommand(RemoveBook, CanRemoveBook);
        EditBookCommand = new RelayCommand(EditBook, CanRemoveBook);
        FindBookCommand = new RelayCommand(FindBook, CanOperateWithBooks);
        ToggleViewCommand = new RelayCommand(ToggleView);

        ExitApplicationCommand = new DelegateCommand<Window>(window => Application.Current.Shutdown());

        LibraryViewHeight = new GridLength(1, GridUnitType.Star);
        LogViewHeight = new GridLength(0);
        ViewName = "Toggle to Log";

        LibraryVisibility = Visibility.Collapsed;
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
    /// Command to close the current library.
    /// </summary>
    public RelayCommand CloseLibraryCommand
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
    /// Command to edit a book in the library.
    /// </summary>
    public RelayCommand EditBookCommand
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
    public ILibrary Library => _libraryManager;

    public Visibility LibraryVisibility
    {
        get => _libraryVisibility;
        set => SetProperty(ref _libraryVisibility, value);
    }
    private Visibility _libraryVisibility;


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

        MessageHandler.SendToStatusBar(_statusBarKind, $"View was switched to {nameView}");
    }

    /// <summary>
    /// Adds a new book to the library.
    /// </summary>
    private void AddBook()
    {
        TextLog += "\n----\n";
        var addBookView = new AddBookViewModel(_libraryManager);
        addBookView.ShowDialog();

        if (addBookView.Book is Book book)
            TextLog += $"Last added book was '{book.Title}'\n";

        TextLog += $"Total books:{_libraryManager?.TotalBooks}";
    }

    /// <summary>
    /// Adds randomly filled books to the library.
    /// </summary>
    private void AddRandomBooks()
    {
        counterUsingAddRandomBooks++;
        TextLog += "\n----\n";
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
        var text = $"Added 10 random named books. Total books:{_libraryManager?.TotalBooks}";
        TextLog += text;

        MessageHandler.SendToStatusBar(_statusBarKind, text);
    }

    private void EditBook()
    {
        var editBookView = new EditBookViewModel(_libraryManager, _libraryManager.SelectedBook);
        editBookView.ShowDialog();
        RaisePropertyChanged(nameof(_libraryManager.BookList));
        if (editBookView.Book is Book book)
            TextLog += $"Last edited book was '{book.Title}'\n";
    }

    /// <summary>
    /// Creates a new library or changes an instance of the existing library by creating a new one.
    /// </summary>
    private void CreateNewLibrary()
    {
        counterUsingAddRandomBooks = 0;
        UnsubscribeTotalBooksChanged();

        _libraryManager.CreateNewLibrary(Random.Shared.Next());
        var text = $"Created a new library with id: {_libraryManager.Id}";
        TextLog += text;

        SubscribeTotalBooksChanged();

        MessageHandler.SendToStatusBar(_statusBarKind, text);
    }

    /// <summary>
    /// Loads an existing library from a file.
    /// </summary>
    private void LoadLibrary()
    {
        counterUsingAddRandomBooks = 0;
        UnsubscribeTotalBooksChanged();

        var filePath = new SelectionDialogHandler().GetPathToXmlFile();

        if (_libraryManager.LoadLibrary(new XmlLibraryLoader(), filePath))
        {
            var text = $"Library was loaded with id:{_libraryManager.Id}. Total books:{_libraryManager?.TotalBooks}. Library's path: {filePath}";
            TextLog += text;

            MessageHandler.SendToStatusBar(_statusBarKind, text);
        }
        else
        {
            MessageBox.Show("Library was not loaded");
        }

        SubscribeTotalBooksChanged();
    }

    /// <summary>
    /// Deletes a book from the library.
    /// </summary>
    private void RemoveBook()
    {
        // TODO : replace by ILogger through DI
        TextLog += "\n----";

        var deletedBookId = _libraryManager.SelectedBook?.Id;
        var result = _libraryManager.RemoveBook(_libraryManager.SelectedBook);
        var text = result
            ? $"Deleted book with id: {deletedBookId}"
            : "Nothing to delete";
        TextLog += text;

        MessageHandler.SendToStatusBar(_statusBarKind, text);
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

            var pathToFile = Path.Combine(selectedFolder, $"{_libraryManager.Id}.xml");
            var file = new FileInfo(pathToFile);
            if (file.Exists)
                file.Delete();

            var result = _libraryManager.SaveLibrary(new XmlBookListSaver(), pathToFile);

            var text = result
                    ? $"Saved Library with id:{_libraryManager.Id}. Total books:{_libraryManager?.TotalBooks}. Library's path:{pathToFile}"
            : "Library wasn't saved";
            TextLog += text;
            MessageHandler.SendToStatusBar(_statusBarKind, text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// Sorts the books in the library.
    /// </summary>
    private void SortLibrary()
    {
        _libraryManager.SortLibrary();
        MessageHandler.SendToStatusBar(_statusBarKind, "Library was sorted");
    }

    /// <summary>
    /// Closes the library and clears the book list.
    /// </summary>
    private void CloseLibrary()
    {
        if (_libraryManager != null)
        {
            UnsubscribeTotalBooksChanged();

            _libraryManager.CloseLibrary();
        }
    }

    /// <summary>
    /// Finds books in the library that contain the specified title part.
    /// </summary>
    private void FindBook() => new FindBookViewModel(_libraryManager);

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

    private void LibraryTotalBooksChanged(object? sender, TotalBooksEventArgs e)
    {
        MessageHandler.SendToStatusBar(EInfoKind.TotalPages, $"{e?.TotalBooks ?? 0}");
    }
    private void UnsubscribeTotalBooksChanged()
    {
        if (_libraryManager != null)
            _libraryManager.TotalBooksChanged -= LibraryTotalBooksChanged;

        LibraryVisibility = Visibility.Collapsed;
        MessageHandler.SendToStatusBar(EInfoKind.TotalPages, $"{0}");
    }

    private void SubscribeTotalBooksChanged()
    {
        if (_libraryManager != null)
        {
            if (_libraryManager.BookList is null)
                LibraryVisibility = Visibility.Collapsed;
            else
                LibraryVisibility = Visibility.Visible;

            _libraryManager.TotalBooksChanged += LibraryTotalBooksChanged;
            MessageHandler.SendToStatusBar(EInfoKind.TotalPages, $"{_libraryManager.TotalBooks}");
        }
        else
        {
            LibraryVisibility = Visibility.Collapsed;
        }
    }
    #endregion

    #region Private Members
    private int counterUsingAddRandomBooks = 0;
    private readonly string[] tenWords = ["a", "A", "b", "B", "c", "C", "e", "E", "f", "F"];
    private readonly LibraryBookManagerModel _libraryManager;
    private readonly EWindowKind _statusBarKind;
    #endregion
}
