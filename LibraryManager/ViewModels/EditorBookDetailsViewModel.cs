using System.Windows;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;
using LibraryManager.Views;

namespace LibraryManager.ViewModels;

/// <summary>
/// View model for editing book details.
/// </summary>
/// <author>YR 2025-01-28</author>
internal sealed class EditorBookDetailsViewModel : CreatorBookDetailsViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EditorBookDetailsViewModel"/> class.
    /// </summary>
    /// <param name="bookManager">The book manager instance.</param>
    /// <param name="editedBook">The book being edited.</param>
    public EditorBookDetailsViewModel(IBookManageable bookManager, SettingsModel settings, Book editedBook) : base(bookManager, settings)
    {
        Book = editedBook;
        _originalBook = (Book)editedBook.Clone();
    }


    #region Methods
    /// <summary>
    /// Shows the edit book dialog.
    /// </summary>
    public override void ShowDialog()
    {
        ExecuteButtonName = Constants.SAVE_CHANGES;
        WindowTitle = Constants.EDIT_BOOK;

        var isContentNull = Book.Content is null;
        LoadingState = isContentNull ? Constants.LOAD_CONTENT : Constants.CONTENT_WAS_LOADED;
        IsLoadEnabled = isContentNull;
        IsSaveEnabled = !IsLoadEnabled;

        ExecuteCommand = new DelegateCommand<Window>(SaveEditedBook);
        CancelCommand = new DelegateCommand<Window>(CancelEditBook);

        MessageHandler.PublishDebugMessage($"'{_originalBook.Title}' ({Constants.ID}: {_originalBook.Id}') {Constants.LOADED_FOR_EDITING}");
        MessageHandler.PublishDebugMessage($"{Constants.ORIGINAL_STATE_BOOK}: '{_originalBook}'");

        _editBookWindow = new EditBookDetailsWindow() { DataContext = this };

        _editBookWindow.ShowDialog();
    }


    /// <summary>
    /// Saves the edited book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void SaveEditedBook(Window window)
    {
        MessageHandler.PublishMessage($"{Constants.LAST_EDITED_BOOK} '{Book.Title}'");
        CloseWindow(window);
    }

    /// <summary>
    /// Cancels editing the book and closes the window.
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
        Book.ISBN = _originalBook.ISBN;
        Book.Content = _originalBook.Content;

        MessageHandler.PublishMessage(Constants.EDITING_BOOK_WAS_CANCELLED);
        CloseWindow(window);
    }

    /// <summary>
    /// Closes the specified window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CloseWindow(Window window)
    {
        window?.Close();
        _editBookWindow?.Close();
    }
    #endregion


    #region Fields
    private EditBookDetailsWindow _editBookWindow;
    private Book _originalBook;
    #endregion
}