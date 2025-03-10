﻿using System.IO;
using BookLibraryManager.Common;
using LibraryManager.Events;
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
    public LibraryViewModel(ILibraryManageable libraryManager, SettingsModel settings)
    {
        _libraryManager = libraryManager;

        CreateLibraryCommand = new DelegateCommand(CreateLibrary);
        LoadLibraryCommand = GetDelegateCommandWithLockAsync(LoadLibraryAsXml);
        SaveLibraryCommand = GetDelegateCommandWithLockAsync(SaveLibraryAsXml);
        SaveAsLibraryCommand = GetDelegateCommandWithLockAsync(SaveAsLibraryAsXml);
        CloseLibraryCommand = new DelegateCommand(CloseLibrary);

        _libraryManager.TotalBooksChanged += Handle_TotalBooksChanged;
        _libraryManager.Library.LibraryIdChanged += Handle_LibraryIdChanged;

        App.EventAggregator.GetEvent<ApplicationShutdownEvent>().Subscribe(HandleApplicationShutdownEvent);

        UpdateLibraryState();
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

    /// <summary>
    /// Gets or sets a value indicating whether operations can be performed on the books in the library.
    /// </summary>
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
    public string CreateLibraryTooltip => Constants.CREATE_NEW_LIBRARY;

    /// <summary>
    /// Command to load an existing library.
    /// </summary>
    public DelegateCommand LoadLibraryCommand
    {
        get;
    }
    public string LoadLibraryTooltip => Constants.LIBRARY_LOAD;

    /// <summary>
    /// Command to save the current library.
    /// </summary>
    public DelegateCommand SaveLibraryCommand
    {
        get;
    }
    public string SaveLibraryTooltip => Constants.LIBRARY_SAVE;

    /// <summary>
    /// Command to save the current library.
    /// </summary>
    public DelegateCommand SaveAsLibraryCommand
    {
        get;
    }
    public string SaveAsLibraryTooltip => Constants.LIBRARY_SAVE_WITH_NEW_NAME;

    /// <summary>
    /// Command to close the current library.
    /// </summary>
    public DelegateCommand CloseLibraryCommand
    {
        get;
    }
    public string CloseLibraryTooltip => Constants.LIBRARY_CLOSE;
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
    /// Creates a new library or changes an instance of the existing library by creating a new one.
    /// </summary>
    private void CreateLibrary()
    {
        if (HasLibraryHashCodeChanged())
            return;

        _libraryManager.CreateNewLibrary(Random.Shared.Next());

        UpdateLibraryState();

        MessageHandler.PublishMessage($"{Constants.LIBRARY_WAS_CREATED_SUCCESSFULLY} with ID: {Library.Id}");
    }

    /// <summary>
    /// Loads an existing library from the disk.
    /// </summary>
    private async Task LoadLibraryAsXml()
    {
        if (HasLibraryHashCodeChanged())
            return;

        var xmlFilePath = new SelectionDialogHandler().GetPathToXmlFile(Constants.LIBRARY_LOAD);

        MessageHandler.PublishMessage(Constants.LOADING_LIBRARY_FROM_XML);

        // XML provider of loading library
        var result = await TaskHandler.TryExecuteTaskAsync(()
            => Task.FromResult(_libraryManager.TryLoadLibrary(new XmlLibraryLoader(), xmlFilePath)));

        if (result?.Result ?? false)
        {
            MessageHandler.PublishTotalBooksInLibrary(Library?.TotalBooks ?? 0);
            MessageHandler.PublishMessage($"{Constants.LIBRARY_LOADED_WITH_ID}: {Library?.Id}");
        }
        else
        {
            MessageHandler.PublishDebugMessage($"{Constants.FAILED_TO_LOAD_LIBRARY_FROM_PATH}: '{xmlFilePath}'");
            new MessageBoxHandler().Show(Constants.LIBRARY_WAS_NOT_LOADED);
        }

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
        window.ShowInput(Constants.INPUT_NEW_NAME_LIBRARY, Constants.INPUT_NAME);
        if (window.DialogResult == Models.EDialogResult.YesButton && window.InputString is string libraryName && !string.IsNullOrWhiteSpace(libraryName))
        {
            await TrySaveLibraryAsXml(libraryName);
        }
        else
        {
            // TODO : create version with displaying error message - "Error"
            new MessageBoxHandler().Show(Constants.OPERATION_WAS_CANCELED);
        }
    }

    /// <summary>
    /// Tries to save the library as XML.
    /// </summary>
    private async Task TrySaveLibraryAsXml(string? libraryName)
    {
        var msg = string.Empty;
        var pathToFile = string.Empty;
        try
        {
            var selectedFolder = new SelectionDialogHandler().GetPathToFolder(Constants.LIBRARY_SAVE);
            if (string.IsNullOrEmpty(selectedFolder))
                throw new Exception(Constants.FOLDER_WAS_NOT_SELECTED);

            var fileName = string.IsNullOrWhiteSpace(libraryName) ? Library.Id.ToString() : libraryName;

            pathToFile = Path.Combine(selectedFolder, StringsHandler.CreateXmlFileName(fileName));

            if (File.Exists(pathToFile))
                File.Delete(pathToFile);

            MessageHandler.PublishMessage(Constants.LIBRARY_IS_BEING_SAVED);
            await Task.Yield();

            // XML provider of saving library
            var result = await TaskHandler.TryExecuteTaskAsync(()
                => Task.FromResult(_libraryManager.TrySaveLibrary(new XmlLibraryKeeper(), pathToFile)));

            msg = result?.Result ?? false ?
                $"{Constants.LIBRARY_WAS_SAVED_SUCCESSFULLY}: '{pathToFile}'" :
                $"{Constants.FAILED_TO_SAVE_LIBRARY_TO_PATH}: '{pathToFile}'";

            if (result?.Result ?? false && string.IsNullOrWhiteSpace(libraryName))
                _libraryHashCode = Library.GetHashCode();
        }
        catch
        {
            msg = $"{Constants.FAILED_TO_SAVE_LIBRARY_TO_PATH} '{pathToFile}'";
        }
        new MessageBoxHandler().Show(msg);
    }

    /// <summary>
    /// Closes the library and clears the book list.
    /// </summary>
    private void CloseLibrary()
    {
        if (_libraryManager != null)
        {
            if (HasLibraryHashCodeChanged())
                return;

            var id = Library.Id;

            _libraryManager.TryCloseLibrary();
            UpdateLibraryState();

            MessageHandler.PublishMessage($"'{id}' {Constants.LIBRARY_WAS_CLOSED}");
        }
    }

    /// <summary>
    /// Returns a DelegateCommand that locks the buttons while executing the specified asynchronous function.
    /// </summary>
    /// <param name="func">The asynchronous function to execute, of type Func<Task>.</param>
    /// <param name="isLocked">A boolean property that indicates whether the buttons are locked.</param>
    /// <returns>A DelegateCommand that locks the buttons while executing the specified asynchronous function.</returns>
    private DelegateCommand GetDelegateCommandWithLockAsync(Func<Task> func) => new(async () =>
    {
        try
        {
            IsUnLocked = false;
            await func().ConfigureAwait(false);
        }
        finally
        {
            IsUnLocked = true;
        }
    });


    /// <summary>
    /// Handles the TotalBooksChanged event by sending message to the status bar with the new total number of books.
    /// </summary>
    /// <param name="e">The event arguments containing the new total number of books and a kind of the event.</param>
    private void Handle_TotalBooksChanged(object? sender, TotalBooksEventArgs e)
    {
        MessageHandler.PublishTotalBooksInLibrary(e?.TotalBooks ?? 0);
    }

    /// <summary>
    /// Handles the LibraryIdChanged event by updating the CanOperateWithBooks property.
    /// </summary>
    private void Handle_LibraryIdChanged(object? sender, EventArgs e)
    {
        CanOperateWithBooks = (sender as ILibrary)?.Id != 0;
    }

    /// <summary>
    /// Updates the library state by raising a property changed event for the Library property
    /// and setting the IsEnabled property based on whether the Library Id differs from the default value of 0.
    /// </summary>
    private void UpdateLibraryState()
    {
        RaisePropertyChanged(nameof(Library));

        _libraryHashCode = Library.GetHashCode();
    }

    /// <summary>
    /// Checks if the library hash code has changed and prompts the user to save changes if necessary.
    /// </summary>
    /// <returns>True if the user confirms saving changes, false otherwise.</returns>
    private bool HasLibraryHashCodeChanged()
    {
        var currentHash = Library.GetHashCode();
        var libraryVhanged = currentHash != 0 && currentHash != _libraryHashCode;

        if (libraryVhanged)
        {
            var msgBox = new MessageBoxHandler();
            msgBox.Show(Constants.LIBRARY_SAVE, StringsHandler.LibraryChangedMessage(), EMessageBoxButtonsViewSelector.YesNo);
            return Models.EDialogResult.YesButton == msgBox.DialogResult;
        }
        return false;
    }

    /// <summary>
    /// Handles the ApplicationShutdownEvent by checking if the library hash code has changed and saving the library as XML if necessary.
    /// </summary>
    /// <param name="args">The event arguments containing information about the application shutdown.</param>
    private void HandleApplicationShutdownEvent(ApplicationShutdownEventArgs args)
    {
        if (HasLibraryHashCodeChanged())
            TrySaveLibraryAsXml(null).ConfigureAwait(false);
    }
    #endregion


    #region Private Members
    private readonly ILibraryManageable _libraryManager;
    private bool _isChecked;
    private bool _canOperateWithBooks;
    private bool _isUnLocked = true;
    private int _libraryHashCode;
    #endregion
}
