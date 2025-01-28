using System.Windows;
using BookLibraryManager.Common;
using BookLibraryManager.DemoApp.Events;
using BookLibraryManager.TestApp.View;
using BookLibraryManager.TestApp.ViewModel;

namespace BookLibraryManager.DemoApp.ViewModel;

/// <author>YR 2025-01-28</author>
public class EditBookViewModel : AddBookViewModel
{

    public EditBookViewModel(ILibrary libraryManager, Book editedBook) : base(libraryManager)
    {
        Book = editedBook;
        _originalBook = (Book)editedBook.Clone();
    }


    #region Methods
    public override void ShowDialog()
    {
        ExecuteButtonName = "Save Book";
        WindowTitle = "Edit Book";

        ExecuteCommand = new DelegateCommand<Window>(SaveEditedBook);
        CancelCommand = new DelegateCommand<Window>(CancelEditBook);

        _addBookWindow = new ActionWithBookWindow() { DataContext = this };
        _addBookWindow.ShowDialog();
    }


    /// <summary>
    /// Adds the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void SaveEditedBook(Window window)
    {
        MessageHandler.SendToStatusBar(StatusBarKindEnum.MainWindow, $"The last edited book '{Book.Title}'");
        CloseWindow(window);
    }

    /// <summary>
    /// Cancels adding the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CancelEditBook(Window window)
    {
        Book.Id = _originalBook.Id;
        Book.Title = _originalBook.Title;
        Book.Author = _originalBook.Author;
        Book.PublishDate = _originalBook.PublishDate;
        Book.TotalPages = _originalBook.TotalPages;
        Book.Description = _originalBook.Description;
        Book.ISDN = _originalBook.ISDN;
        Book.Content = _originalBook.Content;

        MessageHandler.SendToStatusBar(StatusBarKindEnum.MainWindow, $"Edit book '{Book.Title}' was cancelled");
        CloseWindow(window);
    }

    /// <summary>
    /// Closes the specified window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CloseWindow(Window window)
    {
        window?.Close();
        _addBookWindow?.Close();
    }
    #endregion

    #region Fields
    private ActionWithBookWindow _addBookWindow;
    private Book _originalBook;
    #endregion
}