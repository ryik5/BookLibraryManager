using System.Windows;
using BookLibraryManager.Common;
using BookLibraryManager.TestApp.View;
using GalaSoft.MvvmLight.CommandWpf;

namespace BookLibraryManager.TestApp.ViewModel;
public class AddBookViewModel : BindableBase
{
    public AddBookViewModel(Book book)
    {
        Book = book;
        _originalBook = new() { Id = book.Id, Author = book.Author, Title = book.Title, PageNumber = book.PageNumber };
        AddCommand = new RelayCommand<Window>(AddBook);
        CancelCommand = new RelayCommand<Window>(CancelAddBook);

        _addBookWindow = new AddBookWindow() { DataContext = this };
        _addBookWindow.ShowDialog();
    }

    public bool IsAddNewBook
    {
        get;private set;
    }

    public Book Book
    {
        get => _book;
        set => SetProperty(ref _book, value);
    }
    private Book _book;


    public RelayCommand<Window> AddCommand
    {
        get;
    }
    private void AddBook(Window window)
    {
        IsAddNewBook = true;
        CloseWindow(window);
    }


    public RelayCommand<Window> CancelCommand
    {
        get;
    }
    private void CancelAddBook(Window window)
    {
        IsAddNewBook = false;
        Book.Id = _originalBook.Id;
        Book.Author = _originalBook.Author;
        Book.Title = _originalBook.Title;
        Book.PageNumber = _originalBook.PageNumber;
        CloseWindow(window);
    }


    private void CloseWindow(Window window)
    {
        window?.Close();
        _addBookWindow?.Close();
    }

    private AddBookWindow _addBookWindow;
    private Book _originalBook;
}
