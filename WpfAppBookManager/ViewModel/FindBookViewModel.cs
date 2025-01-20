using System.Windows;
using BookLibraryManager.Common;
using BookLibraryManager.TestApp.View;
using GalaSoft.MvvmLight.CommandWpf;

namespace BookLibraryManager.TestApp.ViewModel;

public class FindBookViewModel:BindableBase
{
    public FindBookViewModel(BookLibraryManager libraryManager, ILibrary library)
    {
        _libraryManager = libraryManager;
        _library=library;
        FindBooksCommand = new RelayCommand(FindBooks);
      var finderWindow=  new FindBookWindow() { DataContext = this }.ShowDialog();

        // TODO : add command - delete selected book
        // TODO : change library view to selectable
    }


    public RelayCommand FindBooksCommand
    {
        get;
    }
    private void FindBooks()
    {
        BookList= _libraryManager.FindBooksByTitle(_library, SearchText);
    }

    public string SearchText
    {
        get; set;
    }

    public string TextLog
    {
        get; set;
    }

    public List<Book> BookList
    {
        get => _bookList;
        set => SetProperty(ref _bookList, value);
    }
    private List<Book> _bookList;

    public Book SelectedBook
    {
        get => _selectedBook;
        set => SetProperty(ref _selectedBook, value);
    }
    private Book _selectedBook;


    /// <summary>
    /// Closes the specified window.
    /// </summary>
    private void CloseWindow(Window window)
    {
        window?.Close();
        _finderWindow?.Close();
    }

    private ActionWithBookWindow _finderWindow;

    private readonly BookLibraryManager _libraryManager;
    private readonly ILibrary _library;
}
