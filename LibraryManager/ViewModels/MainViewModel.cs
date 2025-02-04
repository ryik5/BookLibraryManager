using System.IO;
using System.Windows;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;

namespace LibraryManager.ViewModels;

/// <author>YR 2025-02-02</author>
public class MainViewModel : BindableBase, IViewModelPageable
{
    public MainViewModel(LibraryBookManagerModel libraryManager)
    {
        _libraryManager = libraryManager;
        CreateNewCommand = new DelegateCommand(CreateNewLibrary);
        LoadLibraryCommand = new DelegateCommand(LoadLibrary);
        SaveLibraryCommand = new RelayCommand(SaveLibrary, CanOperateWithBooks);
        SortLibraryCommand = new RelayCommand(SortLibrary, CanOperateWithBooks);
        CloseLibraryCommand = new RelayCommand(CloseLibrary, CanOperateWithBooks);

        AddBookCommand = new RelayCommand(AddBook, CanOperateWithBooks);
        AddRandomBooksCommand = new RelayCommand(AddRandomBooks, CanOperateWithBooks);
        RemoveBookCommand = new RelayCommand(RemoveBook, CanRemoveBook);
        EditBookCommand = new RelayCommand(EditBook, CanRemoveBook);

        LibraryVisibility = Visibility.Collapsed;
    }


    #region Properties
    public string Name => "Main";

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }

    public ILibrary Library => _libraryManager;

    public Visibility LibraryVisibility
    {
        get => _libraryVisibility;
        set => SetProperty(ref _libraryVisibility, value);
    }
    #endregion


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
    /// Command to edit a book in the library.
    /// </summary>
    public RelayCommand EditBookCommand
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
    /// Adds a new book to the library.
    /// </summary>
    private void AddBook()
    {
        new AddBookViewModel(_libraryManager).ShowDialog();
    }

    /// <summary>
    /// Adds randomly filled books to the library.
    /// </summary>
    private void AddRandomBooks()
    {
        for (var i = 0; i < 10; i++)
            _libraryManager.AddBook(AddBookViewModel.CreateDemoBook());

        var text = $"Added 10 randomly generated books. Total books:{_libraryManager?.TotalBooks}";

        MessageHandler.SendToStatusBar(text);
    }


    /// <summary>
    /// Creates a new library or changes an instance of the existing library by creating a new one.
    /// </summary>
    private void CreateNewLibrary()
    {
        UnsubscribeTotalBooksChanged();

        _libraryManager.CreateNewLibrary(Random.Shared.Next());
        var text = $"Created a new library with id: {_libraryManager.Id}";

        SubscribeTotalBooksChanged();

        MessageHandler.SendToStatusBar(text);
    }

    /// <summary>
    /// Loads an existing library from a file.
    /// </summary>
    private void LoadLibrary()
    {
        UnsubscribeTotalBooksChanged();

        var filePath = new SelectionDialogHandler().GetPathToXmlFile();

        if (_libraryManager.LoadLibrary(new XmlLibraryLoader(), filePath))
        {
            var text = $"Library was loaded with id:{_libraryManager.Id}. Total books:{_libraryManager?.TotalBooks}. Library's path: {filePath}";

            MessageHandler.SendToStatusBar(text);
        }
        else
        {
            MessageBox.Show("Library was not loaded");
        }

        SubscribeTotalBooksChanged();
    }

    private void EditBook()
    {
        var editBookView = new EditBookViewModel(_libraryManager, _libraryManager.SelectedBook);
        editBookView.ShowDialog();
        RaisePropertyChanged(nameof(_libraryManager.BookList));
    }

    /// <summary>
    /// Deletes a book from the library.
    /// </summary>
    private void RemoveBook()
    {
        var deletedBookId = _libraryManager.SelectedBook?.Id;
        var result = _libraryManager.RemoveBook(_libraryManager.SelectedBook);
        var text = result
            ? $"Deleted book with id: {deletedBookId}"
            : "Nothing to delete";

        MessageHandler.SendToStatusBar(text);
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
            MessageHandler.SendToStatusBar(text);
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
        MessageHandler.SendToStatusBar("Library was sorted");
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

    private void LibraryTotalBooksChanged(object? sender, TotalBooksEventArgs e)
    {
        MessageHandler.SendToStatusBar($"{e?.TotalBooks ?? 0}", EInfoKind.TotalPages);
    }

    private void UnsubscribeTotalBooksChanged()
    {
        if (_libraryManager != null)
            _libraryManager.TotalBooksChanged -= LibraryTotalBooksChanged;

        LibraryVisibility = Visibility.Collapsed;
        MessageHandler.SendToStatusBar($"{0}", EInfoKind.TotalPages);
    }

    private void SubscribeTotalBooksChanged()
    {
        if (_libraryManager != null)
        {
            LibraryVisibility = _libraryManager.BookList is null ? Visibility.Collapsed : LibraryVisibility = Visibility.Visible;

            _libraryManager.TotalBooksChanged += LibraryTotalBooksChanged;
            MessageHandler.SendToStatusBar($"{_libraryManager.TotalBooks}", EInfoKind.TotalPages);
        }
        else
        {
            LibraryVisibility = Visibility.Collapsed;
        }
    }
    #endregion


    #region Private Members
    private readonly LibraryBookManagerModel _libraryManager;
    private bool _isChecked;
    private Visibility _libraryVisibility;
    #endregion
}
