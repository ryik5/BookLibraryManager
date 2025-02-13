﻿using System.Windows;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;
using LibraryManager.Views;

namespace LibraryManager.ViewModels;

/// <author>YR 2025-01-28</author>
internal class EditBookViewModel : ActionWithBookViewModel
{

    public EditBookViewModel(IBookManageable libraryManager, Book editedBook) : base(libraryManager)
    {
        Book = editedBook;
        _originalBook = (Book)editedBook.Clone();
    }


    #region Methods
    public override void ShowDialog()
    {
        ExecuteButtonName = "Save changes";
        WindowTitle = "Edit Book";

        var isContentNull = Book.Content is null;
        LoadingState = isContentNull ? "Load content" : "Content was loaded";
        IsLoadEnabled = isContentNull;
        IsSaveEnabled = !IsLoadEnabled;

        ExecuteCommand = new DelegateCommand<Window>(SaveEditedBook);
        CancelCommand = new DelegateCommand<Window>(CancelEditBook);

        MessageHandler.SendToStatusBar($"The book '{_originalBook.Title}' (ID {_originalBook.Id}') was loaded for editing", EInfoKind.DebugMessage);
        MessageHandler.SendToStatusBar($"Original state of the book: '{_originalBook}'", EInfoKind.DebugMessage);

        _editBookWindow = new ActionWithBookWindow() { DataContext = this };

        _editBookWindow.ShowDialog();
    }


    /// <summary>
    /// Adds the book and closes the window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void SaveEditedBook(Window window)
    {
        MessageHandler.SendToStatusBar($"After editing the book has look: '{Book}'", EInfoKind.DebugMessage);
        MessageHandler.SendToStatusBar($"The last edited book '{Book.Title}'");
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
        Book.ISBN = _originalBook.ISBN;
        Book.Content = _originalBook.Content;

        MessageHandler.SendToStatusBar($"Edit of book '{_originalBook.Title}' (ID {_originalBook.Id}') was cancelled");
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
    private ActionWithBookWindow _editBookWindow;
    private Book _originalBook;
    #endregion
}