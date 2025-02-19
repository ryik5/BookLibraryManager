﻿using System.IO;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;

namespace LibraryManager.ViewModels;

/// <summary>
/// LibraryViewModel for the library manager application.
/// </summary>
/// <author>YR 2025-02-02</author>
internal sealed class LibraryViewModel : BindableBase, IViewModelPageable
{
    /// <summary>
    /// Initializes a new instance of the LibraryViewModel class.
    /// </summary>
    /// <param name="libraryManager">The library manager model.</param>
    public LibraryViewModel(ILibraryManageable libraryManager)
    {
        _libraryManager = libraryManager;
        CreateLibraryCommand = new DelegateCommand(CreateLibrary);
        LoadLibraryCommand = GetDelegateCommandWithLockAsync(LoadLibraryAsXml);
        SaveLibraryCommand = GetDelegateCommandWithLockAsync(SaveLibraryAsXml);
        SaveAsLibraryCommand = GetDelegateCommandWithLockAsync(SaveAsLibraryAsXml);
        CloseLibraryCommand = new DelegateCommand(CloseLibrary);
        UpdateLibraryState();
        _libraryManager.TotalBooksChanged += LibraryTotalBooksChanged;
        _libraryManager.Library.LibraryIdChanged += Library_LibraryIdChanged;
    }


    #region Properties
    public string Name => Constants.LIBRARY;

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the buttons are unlocked.
    /// </summary>
    public bool IsUnLocked
    {
        get => _isUnLocked;
        set => SetProperty(ref _isUnLocked, value);
    }

    public bool CanOperateWithBooks
    {
        get => _canOperateWithBooks;
        set => SetProperty(ref _canOperateWithBooks, value);
    }

    /// <summary>
    /// Gets the library manager model.
    /// </summary>
    public ILibrary Library => _libraryManager.Library;
    #endregion


    #region Commands
    /// <summary>
    /// Command to create a new library.
    /// </summary>
    public DelegateCommand CreateLibraryCommand
    {
        get;
    }
    public string CreateLibraryTooltip =>"Create a new library";

    /// <summary>
    /// Command to load an existing library.
    /// </summary>
    public DelegateCommand LoadLibraryCommand
    {
        get;
    }
    public string LoadLibraryTooltip =>"Load a library";

    /// <summary>
    /// Command to save the current library.
    /// </summary>
    public DelegateCommand SaveLibraryCommand
    {
        get;
    }
    public string SaveLibraryTooltip =>"Save the library";

    /// <summary>
    /// Command to save the current library.
    /// </summary>
    public DelegateCommand SaveAsLibraryCommand
    {
        get;
    }
    public string SaveAsLibraryTooltip =>"Save the library with a new name";

    /// <summary>
    /// Command to close the current library.
    /// </summary>
    public DelegateCommand CloseLibraryCommand
    {
        get;
    }
    public string CloseLibraryTooltip =>"Close the current library";
    #endregion
    /*
      <Button Content="New Library" 
        <Button Content="Load Library" 
        <Button Content="Save Library" 
        <Button Content="SaveAs Library" 
        <Button Content="Close Library" 
*/

    #region Methods
    /// <summary>
    /// Locks the buttons while executing the specified asynchronous function.
    /// </summary>
    /// <param name="func">The asynchronous function to execute.</param>
    private async Task LockButtonsOnExecuteAsync(Func<Task> func)
    {
        IsUnLocked = false;
        await func();
        IsUnLocked = true;
    }

    /// <summary>
    /// Creates a new library or changes an instance of the existing library by creating a new one.
    /// </summary>
    private void CreateLibrary()
    {
        _libraryManager.CreateNewLibrary(Random.Shared.Next());

        UpdateLibraryState();

        MessageHandler.PublishMessage($"{Constants.LIBRARY_WAS_CREATED_SUCCESSFULLY} with ID: {Library.Id}");
        MessageHandler.PublishTotalBooksInLibrary(0);
    }

    /// <summary>
    /// Loads an existing library from a file.
    /// </summary>
    private async Task LoadLibraryAsXml()
    {
        var filePath = new SelectionDialogHandler().GetPathToXmlFile();

        MessageHandler.PublishMessage(Constants.LOADING_LIBRARY_FROM_XML);

        await Task.Yield();

        // XML provider of loading library
        var result = await Handler.TryExecuteTaskAsync(()
            => Task.FromResult(_libraryManager.TryLoadLibrary(new XmlLibraryLoader(), filePath)));

        if (result?.Result ?? false)
        {
            MessageHandler.PublishDebugMessage($"{Constants.LIBRARY_WAS_LOADED_SUCCESSFULLY}: '{filePath}'");
            MessageHandler.PublishTotalBooksInLibrary(Library?.TotalBooks ?? 0);
            MessageHandler.PublishMessage($"{Constants.LIBRARY_LOADED_WITH_ID}: {Library?.Id}");
        }
        else
        {
            MessageHandler.PublishDebugMessage($"{Constants.FAILED_TO_LOAD_LIBRARY_FROM_PATH}: '{filePath}'");

            new MessageBoxHandler().Show(Constants.LIBRARY_WAS_NOT_LOADED);
        }

        MessageHandler.PublishDebugMessage(Constants.LIBRARY_LOADING_FINISHED);
        UpdateLibraryState();
    }

    /// <summary>
    /// Saves the current library to a file.
    /// </summary>
    private async Task SaveLibraryAsXml() => await TrySaveLibraryAsXml(null);

    /// <summary>
    /// Saves the current library with a new name.
    /// </summary>
    private async Task SaveAsLibraryAsXml()
    {
        var window = new MessageBoxHandler();
        window.ShowInput("Input a new name of the library:", "Input library name");
        if (window.DialogResult == Models.DialogResult.YesButton && window.InputString is string libraryName && !string.IsNullOrWhiteSpace(libraryName))
        {
            await TrySaveLibraryAsXml(libraryName);
        }
        else
        {
            new MessageBoxHandler().Show("New library name was not entered"); // TODO : create version with displaying error message - "Error"
        }
    }

    /// <summary>
    /// Tries to save the library as XML.
    /// </summary>
    private async Task TrySaveLibraryAsXml(string? libraryName)
    {
        try
        {
            var selectedFolder = new SelectionDialogHandler().GetPathToFolder(Constants.SAVE_LIBRARY_DIALOG);
            if (string.IsNullOrEmpty(selectedFolder))
                throw new Exception(Constants.FOLDER_WAS_NOT_SELECTED);

            var fileName = string.IsNullOrWhiteSpace(libraryName) ? Library.Id.ToString() : libraryName;

            var pathToFile = Path.Combine(selectedFolder, $"{fileName}.xml");

            var file = new FileInfo(pathToFile);
            if (file.Exists)
                file.Delete();

            MessageHandler.PublishMessage(Constants.LIBRARY_IS_BEING_SAVED);
            await Task.Yield();

            // XML provider of saving library
            var result = await Handler.TryExecuteTaskAsync(()
                => Task.FromResult(_libraryManager.TrySaveLibrary(new XmlLibraryKeeper(), pathToFile)));

            var text = result?.Result ?? false ?
                $"{Constants.LIBRARY_WAS_SAVED_SUCCESSFULLY}: '{pathToFile}'" :
                $"{Constants.FAILED_TO_SAVE_LIBRARY_TO_PATH}: '{pathToFile}'";
            MessageHandler.PublishMessage(text);
        }
        catch (Exception ex)
        {
            new MessageBoxHandler().Show(ex.Message);
        }
    }

    /// <summary>
    /// Closes the library and clears the book list.
    /// </summary>
    private void CloseLibrary()
    {
        if (_libraryManager != null)
        {
            var id = Library.Id;

            _libraryManager.TryCloseLibrary();
            UpdateLibraryState();

            MessageHandler.PublishMessage($"'{id}' {Constants.LIBRARY_WAS_CLOSED}");
        }
    }

    /// <summary>
    /// Updates the library state by raising a property changed event for the Library property
    /// and setting the IsEnabled property based on whether the Library Id differs from the default value of 0.
    /// </summary>
    private void UpdateLibraryState()
    {
        RaisePropertyChanged(nameof(Library));

    }

    /// <summary>
    /// Returns a DelegateCommand that locks the buttons while executing the specified asynchronous function.
    /// </summary>
    /// <param name="func">The asynchronous function to execute, of type Func<Task>.</param>
    /// <returns>A DelegateCommand that locks the buttons while executing the specified asynchronous function.</returns>
    private DelegateCommand GetDelegateCommandWithLockAsync(Func<Task> func) => new(async () => await LockButtonsOnExecuteAsync(func));

    /// <summary>
    /// Handles the TotalBooksChanged event by sending message to the status bar with the new total number of books.
    /// </summary>
    /// <param name="e">The event arguments containing the new total number of books and a kind of the event.</param>
    private void LibraryTotalBooksChanged(object? sender, TotalBooksEventArgs e)
    {
        MessageHandler.PublishTotalBooksInLibrary(e?.TotalBooks ?? 0);
    }

    /// <summary>
    /// Handles the LibraryIdChanged event by updating the CanOperateWithBooks property.
    /// </summary>
    private void Library_LibraryIdChanged(object? sender, EventArgs e)
    {
        CanOperateWithBooks = (sender as ILibrary)?.Id != 0;
    }

    #endregion


    #region Private Members
    private readonly ILibraryManageable _libraryManager;
    private bool _isChecked;
    private bool _canOperateWithBooks;
    private bool _isUnLocked = true;
    #endregion
}
