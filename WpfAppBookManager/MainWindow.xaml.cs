using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using BookLibraryManager;
using BookLibraryManager.Common;
using Microsoft.Win32;

namespace AppBookManager;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        _libraryManager = new BookManager();
    }

    private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        TextLog.Text = string.Empty;
        counterUsingAddRandomBooks = 0;

        _library = _libraryManager.NewLibrary(Random.Shared.Next());
        TextLog.Text = $"New library created with id: {_library.Id}";
    }


    private void LoadCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void LoadCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        TextLog.Text = string.Empty;
        counterUsingAddRandomBooks = 0;

        try
        {
            var filePath = GetPathToXmlFileLibrary();

            _libraryManager.LoadLibrary(new XmlBookListLoader(), filePath, out _library);
            TextLog.Text = $"Library loaded with id: {_library.Id}\n" +
                           $"number of books: {_library.NumberOfBooks}" +
                           $"\nby path: {filePath}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// Return the path to the XML file with the library
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


    private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (_library is null)
        {
            MessageBox.Show("No created library!");
            return;
        }

        TextLog.Text += "\n----";
        var testBook = new Book() { Id = 1, Author = "new", Title = "Test Book", PageNumber = 20 };
        _libraryManager.AddBook(_library, testBook);
        TextLog.Text += $"\nAdded book with id: {testBook.Id}\n" +
                        $"number of books in the library: {_library.NumberOfBooks}";
        TextLog.Text += $"\nLast added books:\n{_library.ShowLastBooks(10)}";
        MyScrollViewer.ScrollToBottom();
    }


    private void AddRandomCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void AddRandomCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (_library is null)
        {
            MessageBox.Show("No created library!");
            return;
        }

        TextLog.Text += "\n----";
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

            _libraryManager.AddBook(_library, testBook);
        }

        TextLog.Text += $"\nAdded 10 books\n" +
                        $"number of books in the library: {_library.NumberOfBooks}";
        TextLog.Text += $"\nLast added books:\n{_library.ShowLastBooks(10)}";
        MyScrollViewer.ScrollToBottom();
    }


    private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (_library is null)
        {
            MessageBox.Show("No created library!");
            return;
        }

        TextLog.Text = $"Library was sorted\nFirst books:\n{_library.ShowFistBooks(10)}";
        TextLog.Text += "\n----";
        var testBook = new Book() { Id = 1, Author = "new", Title = "Test Book", PageNumber = 20 };
        var result = _libraryManager.RemoveBook(_library, testBook);

        TextLog.Text += result
            ? $"\nIt was deleted a book with id: {testBook.Id}\n"
            : $"\nIt was deleted nothing";
        TextLog.Text += $"\nnumber of books in the library: {_library.NumberOfBooks}" +
                        $"\nFirst books:\n{_library.ShowFistBooks(10)}";
        MyScrollViewer.ScrollToBottom();
    }


    private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        TextLog.Text = string.Empty;

        try
        {
            if (_library is null)
                throw new Exception("No library to save!");

            var selectedFolder = GetPathToFolderToStoreLibrary();

            var pathToFile = Path.Combine(selectedFolder, $"{_library.Id}.xml");
            var file = new FileInfo(pathToFile);
            if (file.Exists)
                file.Delete();

            var result = _libraryManager.SaveLibrary(new XmlBookListSaver(), pathToFile, _library);

            TextLog.Text = result
                    ? $"Saved Library with id: {_library.Id}\nnumber of books: {_library.NumberOfBooks}\nLibrary's path: {pathToFile}"
                    : "Library wasn't saved";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    /// <summary>
    /// Return the path to the folder to store the library
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


    private void SortCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void SortCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (_library is null)
        {
            MessageBox.Show("No created library!");
            return;
        }

        _libraryManager.SortLibrary(_library);

        TextLog.Text = $"Library was sorted\nFirst books:\n{_library.ShowFistBooks(10)}";
    }


    private void FindCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = true;
    }
    private void FindCommand_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (_library is null)
        {
            MessageBox.Show("No created library!");
            return;
        }

        var locatedBooks = _libraryManager.FindBooksByTitle(_library, "a");

        TextLog.Text = $"Located books with Title contained - 'a':\n{string.Join("\n", locatedBooks)}";
    }

    /// <summary>
    /// Repeat the word by the times
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
    private ILibrary _library;
    #endregion
}