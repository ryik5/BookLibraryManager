using System.Windows;
using BookLibraryManager.Common.Util;

namespace BookLibraryManager.Common;

/// <summary>
/// Represents a model for managing a library of books.
/// </summary>
/// <author>YR 2025-01-09</author>
public class LibraryManagerModel : BindableBase, ILibraryManageable
{
    public LibraryManagerModel(ILibrary library)
    {
        if (library is null)
            throw new ArgumentNullException(nameof(library));

        _library = library;
        RaisePropertyChanged(nameof(Library));
        Library.BookList.CollectionChanged += BookList_CollectionChanged;
    }


    #region public methods
    /// <summary>
    /// Creates a new library with the specified ID.
    /// </summary>
    /// <param name="idLibrary">The ID of the new library.</param>
    public void CreateNewLibrary(int idLibrary)
    {
        TryCloseLibrary();

        Library.Id = idLibrary;

        RaisePropertyChanged(nameof(Library));
    }

    /// <summary>
    /// Loads a library from the specified path.
    /// </summary>
    /// <param name="libraryLoader">The loader responsible for loading the library.</param>
    /// <param name="pathToLibrary">The path to the storage containing the library data.</param>
    /// <returns>True if the library was successfully loaded; otherwise, false.</returns>
    public bool TryLoadLibrary(ILibraryLoader libraryLoader, string pathToLibrary)
    {
        libraryLoader.LoadingFinished += LibraryLoader_LoadingLibraryFinished;

        TryCloseLibrary();

        var result = libraryLoader.TryLoadLibrary(pathToLibrary, out var library);
        if (result)
        {
            Library.Id = library.Id;
            Library.Name = library.Name;
            Library.Description = library.Description;
            InvokeOnUiThread(() => Library.BookList.ResetAndAddRange(library.BookList));
        }

        libraryLoader.LoadingFinished -= LibraryLoader_LoadingLibraryFinished;

        RaisePropertyChanged(nameof(Library));

        return result;
    }

    /// <summary>
    /// Saves the specified library to the specified folder.
    /// </summary>
    /// <param name="keeper">The keeper responsible for saving the library.</param>
    /// <param name="pathToStorage">The path to the storage where the library will be saved.</param>
    /// <returns>True if the library was successfully saved; otherwise, false.</returns>
    public bool TrySaveLibrary(ILibraryKeeper keeper, string pathToStorage) => keeper.TrySaveLibrary(Library, pathToStorage);

    /// <summary>
    /// Closes the current library.
    /// </summary>
    public void TryCloseLibrary()
    {
        if (0 < Library.BookList.Count)
            InvokeOnUiThread(() => Library.BookList.Clear());

        Library.Name = string.Empty;
        Library.Description = string.Empty;
        Library.Id = 0;
        RaisePropertyChanged(nameof(Library));
    }
    #endregion


    #region Properties
    /// <summary>
    /// Gets or sets a library.
    /// </summary>
    public ILibrary Library
    {
        get => _library;
        set => SetProperty(ref _library, value);
    }

    public event EventHandler<ActionFinishedEventArgs> LoadingFinished;
    public event EventHandler<TotalBooksEventArgs> TotalBooksChanged;
    #endregion


    #region private methods
    /// <summary>
    /// Handles the CollectionChanged event of the BookList.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The NotifyCollectionChangedEventArgs instance containing the event data.</param>
    private void BookList_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        TotalBooksChanged.Invoke(this, new TotalBooksEventArgs { TotalBooks = Library.BookList?.Count ?? 0 });
    }

    /// <summary>
    /// Handles the LoadingFinished event of the LibraryLoader.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The ActionFinishedEventArgs instance containing the event data.</param>
    private void LibraryLoader_LoadingLibraryFinished(object? sender, ActionFinishedEventArgs e)
    {
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = e.Message, IsFinished = e.IsFinished });
    }

    /// <summary>
    /// Invokes the specified action on the UI thread.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    private void InvokeOnUiThread(Action action) => Application.Current?.Dispatcher?.Invoke(action);

    private ILibrary _library;
    #endregion
}
