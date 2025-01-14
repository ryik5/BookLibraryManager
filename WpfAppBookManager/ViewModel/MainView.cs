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
        _libraryManager = new BookLibraryManager();

        ButtonNew = new RelayCommand(CreateNewLibrary);
        ButtonLoader = new RelayCommand(LoadLibrary);

        ButtonSaver = new RelayCommand(SaveLibrary, CanOperateWithBooks);
        ButtonSort = new RelayCommand(SortLibrary, CanOperateWithBooks);

        ButtonAdd = new RelayCommand(AddBook, CanOperateWithBooks);
        ButtonAddRandom = new RelayCommand(AddRandomBooks, CanOperateWithBooks);
        ButtonDelete = new RelayCommand(DeleteBook, CanOperateWithBooks);
        ButtonFind = new RelayCommand(FindBook, CanOperateWithBooks);
    }

    /// <summary>
    /// Gets the command to create a new library.
    /// </summary>
    public RelayCommand ButtonNew { get; }

    /// <summary>
    /// Gets the command to load an existing library.
    /// </summary>
    public RelayCommand ButtonLoader { get; }

    /// <summary>
    /// Gets the command to save the current library.
    /// </summary>
    public RelayCommand ButtonSaver { get; }

    /// <summary>
    /// Gets the command to sort the books in the library.
    /// </summary>
    public RelayCommand ButtonSort { get; }

    /// <summary>
    /// Gets the command to add a new book to the library.
    /// </summary>
    public RelayCommand ButtonAdd { get; }

    /// <summary>
    /// Gets the command to add random books to the library.
    /// </summary>
    public RelayCommand ButtonAddRandom { get; }

    /// <summary>
    /// Gets the command to delete a book from the library.
    /// </summary>
    public RelayCommand ButtonDelete { get; }

    /// <summary>
    /// Gets the command to find books in the library.
    /// </summary>
    public RelayCommand ButtonFind { get; }

    /// <summary>
    /// Gets or sets the text log for displaying logging messages.
    /// </summary>
    public string TextLog
    {
        get => _textLog;
        set => SetProperty(ref _textLog, value);
    }
    private string _textLog;

    /// <summary>
    /// Gets or sets the current library.
    /// </summary>
    public ILibrary Library
    {
        get => _library;
        set => SetProperty(ref _library, value);
    }
    private ILibrary _library;

    /// <summary>
    /// Determines whether operations can be performed on the books in the library.
    /// </summary>
    /// <returns>true if the library has a book list; otherwise, false.</returns>
    private bool CanOperateWithBooks()
    {
        return Library?.BookList != null;
    }

    /// <summary>
    /// Adds a new book to the library.
    /// </summary>
    private void AddBook()
    {
        var book = new Book() { Id = 1, Author = "Author", Title = "Title", PageNumber = 1 };
        var addBook = new AddBookViewModel(book);

        if (addBook.IsAddNewBook)
        {
            _libraryManager.AddBook(Library, book);
            TextLog = $"\nAdded book with id: {book.Id}\n" +
                           $"number of books in the library: {Library.NumberOfBooks}" +
                           $"\nLast added books:\n{Library.ShowLastBooks(10)}";
        }
        else
        {
            TextLog = $"Adding book was canceled";
        }
    }

    /// <summary>
    /// Adds random filled books to the library.
    /// </summary>
    private void AddRandomBooks()
    {
        TextLog = string.Empty;
        counterUsingAddRandomBooks++;

        for (var i = 0; i < 10; i++)
        {
            var testBook = new Book()
            {
                Id = Random.Shared.Next(),
                Author = $"{RepeaterWords(tenWords[tenWords.Length - 1 - i], counterUsingAddRandomBooks)}",
                Title = $"{RepeaterWords(tenWords[i], counterUsingAddRandomBooks)} {Random.Shared.Next()}",
                PageNumber = 20
            };

            _libraryManager.AddBook(Library, testBook);
        }
        TextLog += $"\nAdded 10 books\n" +
                        $"number of books in the library: {Library.NumberOfBooks}";
        TextLog += $"\nLast added books:\n{Library.ShowLastBooks(10)}";
    }

    /// <summary>
    /// Creates a new library\ changes  an instance of the existed library by created one.
    /// </summary>
    private void CreateNewLibrary()
    {
        TextLog = string.Empty;
        counterUsingAddRandomBooks = 0;

        Library = _libraryManager.NewLibrary(Random.Shared.Next());
        TextLog = $"New library created with id: {Library.Id}";
    }

    /// <summary>
    /// Loads an existing library from a file.
    /// </summary>
    private void LoadLibrary()
    {
        TextLog = string.Empty;
        counterUsingAddRandomBooks = 0;

        try
        {
            var filePath = GetPathToXmlFileLibrary();

            _libraryManager.LoadLibrary(new XmlBookListLoader(), filePath, out _library);
            TextLog = $"Library loaded with id: {Library.Id}\n" +
                           $"number of books: {Library.NumberOfBooks}" +
                           $"\nby path: {filePath}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// Returns the path to the XML file with the library.
    /// </summary>
    private string GetPathToXmlFileLibrary()
    {
        var openDialog = new OpenFileDialog()
        {
            Title = "Load the library",
            DefaultExt = ".xml",
            Filter = "XML Books (.xml)|*.xml"
        };
        var dialogResult = openDialog.ShowDialog();
        if (!dialogResult.HasValue || !dialogResult.Value)
            throw new Exception("No selected library to load!");
        var path = openDialog.FileName;

        return path;
    }

    /// <summary>
    /// Deletes a book from the library.
    /// </summary>
    private void DeleteBook()
    {
        TextLog = $"Library was sorted\nFirst books:\n{Library.ShowFistBooks(10)}";
        TextLog += "\n----";
        var testBook = new Book() { Id = 1, Author = "new", Title = "Test Book", PageNumber = 20 };
        var result = _libraryManager.RemoveBook(Library, testBook);
        TextLog += result
            ? $"\nIt was deleted a book with id: {testBook.Id}\n"
            : $"\nIt was deleted nothing";
        TextLog += $"\nnumber of books in the library: {Library.NumberOfBooks}" +
                        $"\nFirst books:\n{Library.ShowFistBooks(10)}";
    }

    /// <summary>
    /// Saves the current library to a file.
    /// </summary>
    private void SaveLibrary()
    {
        TextLog = string.Empty;

        try
        {
            var selectedFolder = GetPathToFolderToStoreLibrary();

            var pathToFile = Path.Combine(selectedFolder, $"{Library.Id}.xml");
            var file = new FileInfo(pathToFile);
            if (file.Exists)
                file.Delete();

            var result = _libraryManager.SaveLibrary(new XmlBookListSaver(), pathToFile, Library);

            TextLog = result
                    ? $"Saved Library with id: {Library.Id}\nnumber of books: {Library.NumberOfBooks}\nLibrary's path: {pathToFile}"
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
    private void SortLibrary()
    {
        _libraryManager.SortLibrary(Library);

        TextLog = $"Library was sorted\nFirst books:\n{Library.ShowFistBooks(10)}";
    }

    /// <summary>
    /// Finds books in the library that contain the specified title part.
    /// </summary>
    private void FindBook()
    {
        var locatedBooks = _libraryManager.FindBooksByTitle(Library, "a");

        TextLog = $"Located books with Title contained - 'a':\n{string.Join("\n", locatedBooks)}";
    }

    /// <summary>
    /// Repeats the word by the specified number of times.
    /// </summary>
    private string RepeaterWords(string word, int times)
    {
        StringBuilder stringBuilder = new();
        for (int i = 0; i < times; i++)
        {
            stringBuilder.Append($"{word}");
        }
        return stringBuilder.ToString();
    }

    #region private members
    private int counterUsingAddRandomBooks = 0;
    private readonly string[] tenWords = ["a", "A", "b", "B", "c", "C", "e", "E", "f", "F"];
    private readonly IBookLibraryManageable _libraryManager;
    #endregion
}
