using System.IO;
using System.Text;
using System.Windows;
using BookLibraryManager.Common;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace BookLibraryManager.TestApp.ViewModel;
/// <summary>
/// ViewModel for the main view of the Book Library Manager application.
/// </summary>
/// <author>YR 2025-01-14</author>
public class MainView : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainView"/> class.
    /// </summary>
    public MainView()
    {
        _libraryManager = new LibraryBookManagerModel();

        CreateNewCommand = new RelayCommand(CreateNewLibrary);
        LoadLibraryCommand = new RelayCommand(LoadLibrary);

        SaveLibraryCommand = new RelayCommand(SaveLibrary, CanOperateWithBooks);
        SortLibraryCommand = new RelayCommand(SortLibrary, CanOperateWithBooks);

        AddBookCommand = new RelayCommand(AddBook, CanOperateWithBooks);
        AddRandomBooksCommand = new RelayCommand(AddRandomBooks, CanOperateWithBooks);
        RemoveBookCommand = new RelayCommand(RemoveBook, CanRemoveBook);
        FindBookCommand = new RelayCommand(FindBook, CanOperateWithBooks);
        ToggleViewCommand = new RelayCommand(ToggleView);

        ExitApplicationCommand = new RelayCommand<Window>(window => Application.Current.Shutdown());

        LibraryViewHeight = new GridLength(1, GridUnitType.Star);
        LogViewHeight = new GridLength(0);
        ViewName = "Toggle to Log";
    }

    #region Commands
    /// <summary>
    /// Command to create a new library.
    /// </summary>
    public RelayCommand CreateNewCommand
    {
        get;
    }

    /// <summary>
    /// Command to load an existing library.
    /// </summary>
    public RelayCommand LoadLibraryCommand
    {
        get;
    }

    /// <summary>
    /// Command to save the current library.
    /// </summary>
    public RelayCommand SaveLibraryCommand
    {
        get;
    }

    /// <summary>
    /// Command to sort the books in the library.
    /// </summary>
    public RelayCommand SortLibraryCommand
    {
        get;
    }

    /// <summary>
    /// Command to add a new book to the library.
    /// </summary>
    public RelayCommand AddBookCommand
    {
        get;
    }

    /// <summary>
    /// Command to add random books to the library.
    /// </summary>
    public RelayCommand AddRandomBooksCommand
    {
        get;
    }

    /// <summary>
    /// Command to delete a book from the library.
    /// </summary>
    public RelayCommand RemoveBookCommand
    {
        get;
    }

    /// <summary>
    /// Command to find books in the library.
    /// </summary>
    public RelayCommand FindBookCommand
    {
        get;
    }

    /// <summary>
    /// Command to exit the application.
    /// </summary>
    public RelayCommand ToggleViewCommand
    {
        get;
    }

    /// <summary>
    /// Command to exit the application.
    /// </summary>
    public RelayCommand<Window> ExitApplicationCommand
    {
        get;
    }
    #endregion

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
    /// Gets or sets the height of the log for displaying logging messages.
    /// </summary>
    public GridLength LogViewHeight
    {
        get => _logViewHeight;
        set => SetProperty(ref _logViewHeight, value);
    }
    private GridLength _logViewHeight;

    /// <summary>
    /// Gets or sets the height of the library.
    /// </summary>
    public GridLength LibraryViewHeight
    {
        get => _libraryViewHeight;
        set => SetProperty(ref _libraryViewHeight, value);
    }
    private GridLength _libraryViewHeight;

    /// <summary>
    /// Determines whether operations can be performed on the books in the library.
    /// </summary>
    /// <returns>true if the library has a book list; otherwise, false.</returns>
    private bool CanOperateWithBooks()
    {
        return _libraryManager?.BookList != null;
    }

    /// <summary>
    /// Determines whether operations can be performed on the book in the library.
    /// </summary>
    /// <returns>true if the library has a book list and selected book is not null; otherwise, false.</returns>
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
        if (LibraryViewHeight.Value > 0)
        {
            LibraryViewHeight = new GridLength(0);
            LogViewHeight = new GridLength(1, GridUnitType.Star);
            ViewName = "Toggle to Library";
        }
        else
        {
            LibraryViewHeight = new GridLength(1, GridUnitType.Star);
            LogViewHeight = new GridLength(0);
            ViewName = "Toggle to Log";
        }
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
            TextLog += $"\nAdded book with id: {book.Id}\n" +
                           $"number of books in the library: {_libraryManager?.NumberOfBooks}";
        }
        else
        {
            TextLog += $"Adding book was canceled";
        }
    }

    /// <summary>
    /// Adds random filled books to the library.
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
        TextLog += $"\nAdded 10 books\n" +
                        $"number of books in the library: {_libraryManager?.NumberOfBooks}";
    }

    /// <summary>
    /// Creates a new library\ changes  an instance of the existed library by created one.
    /// </summary>
    private void CreateNewLibrary()
    {
        counterUsingAddRandomBooks = 0;

        _libraryManager.CreateNewLibrary(Random.Shared.Next());
        TextLog += $"New library created with id: {_libraryManager.Id}";
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
            TextLog += $"Library loaded with id: {_libraryManager.Id}\nnumber of books: {_libraryManager?.NumberOfBooks}\nby path: {filePath}";
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
        TextLog += result
            ? $"\nIt was deleted a book with id: {deletedBookId}\n"
            : $"\nIt was deleted nothing";
        TextLog += $"\nnumber of books in the library: {_libraryManager?.NumberOfBooks}";
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

            TextLog += result
                    ? $"Saved Library with id: {_libraryManager.Id}\nnumber of books: {_libraryManager?.NumberOfBooks}\nLibrary's path: {pathToFile}"
                    : "Library wasn't saved";
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
    private void SortLibrary() => _libraryManager.SortLibrary();

    /// <summary>
    /// Finds books in the library that contain the specified title part.
    /// </summary>
    private void FindBook() => _ = new FindBookViewModel(_libraryManager);

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

    #region private members
    private int counterUsingAddRandomBooks = 0;
    private readonly string[] tenWords = ["a", "A", "b", "B", "c", "C", "e", "E", "f", "F"];
    private readonly LibraryBookManagerModel _libraryManager;
    #endregion
}
