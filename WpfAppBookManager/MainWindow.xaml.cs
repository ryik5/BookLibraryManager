using System.IO;
using System.Windows;
using System.Windows.Input;
using BookLibraryManager;
using BookLibraryManager.Models;
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
        try
        {
            var filePath = GetPathToFolder();

            _libraryManager.LoadLibrary(new XmlBookListLoader(), filePath, out _library);
            TextLog.Text = $"Library loaded with id: {_library.Id}\n" +
                           $"number of books: {_library.AmountBooks}" +
                           $"\nby path: {filePath}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private string GetPathToFolder()
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
                        $"number of books in the library: {_library.AmountBooks}";
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

        for (var i = 0; i < 10; i++)
        {
            var testBook = new Book() { Id = Random.Shared.Next(), Author = $"{Random.Shared.Next()}", Title = $"{Random.Shared.NextDouble()}", PageNumber = 20 };
            _libraryManager.AddBook(_library, testBook);
        }

        TextLog.Text += $"\nAdded 10 books\n" +
                        $"number of books in the library: {_library.AmountBooks}";
        TextLog.Text += $"\nLast added books:\n{_library.ShowLastBooks(10)}";
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
            if(_library is null)
                throw new Exception("No library to save!");

            var selectedFolder = GetPathToFolder2();

            var pathToFile = Path.Combine(selectedFolder, $"{_library.Id}.xml");
            var file = new FileInfo(pathToFile);
            if (file.Exists)
                file.Delete();

            var result = _libraryManager.SaveLibrary(new XmlBookListSaver(), pathToFile, _library);

            TextLog.Text = result
                    ? $"Saved Library with id: {_library.Id}\nnumber of books: {_library.AmountBooks}\nLibrary's path: {pathToFile}"
                    : "Library wasn't saved";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private string GetPathToFolder2()
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
        TextLog.Text += $"\nnumber of books in the library: {_library.AmountBooks}" +
                        $"\nFirst books:\n{_library.ShowFistBooks(10)}";
        MyScrollViewer.ScrollToBottom();
    }


    private readonly IBookLibraryManageable _libraryManager;
    private ILibrary _library;
}