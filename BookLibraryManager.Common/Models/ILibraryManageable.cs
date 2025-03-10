﻿namespace BookLibraryManager.Common;

/// <summary>
/// Represents a library interface that provides functionalities to add, remove, sort, and display Library.
/// </summary>
/// <author>YR 2025-01-09</author>
public interface ILibraryManageable : ILoadable
{
    public event EventHandler<ActionFinishedEventArgs> LoadingFinished;
    public event EventHandler<TotalBooksEventArgs> TotalBooksChanged;

    /// <summary>
    /// Creates a new library with the specified ID.
    /// </summary>
    /// <param name="idLibrary">The ID of the new library.</param>
    void CreateNewLibrary(int idLibrary);

    /// <summary>
    /// Loads a library from the specified path.
    /// </summary>
    /// <param name="libraryLoader">The loader responsible for loading the library.</param>
    /// <param name="pathToLibrary">The path to the file containing the library data.</param>
    /// <returns>True if the library was successfully loaded; otherwise, false.</returns>
    bool TryLoadLibrary(ILibraryLoader libraryLoader, string pathToLibrary);

    /// <summary>
    /// Saves the specified library to the specified folder.
    /// </summary>
    /// <param name="keeper">The keeper responsible for saving the library.</param>
    /// <param name="pathToStorage">The path to the storage where the library will be saved.</param>
    /// <returns>True if the library was successfully saved; otherwise, false.</returns>
    bool TrySaveLibrary(ILibraryKeeper keeper, string pathToStorage);

    /// <summary>
    /// Closes the library and clears the book list.
    /// </summary>
    void TryCloseLibrary();

    /// <summary>
    /// Gets or sets the library.
    /// </summary>
    ILibrary Library
    {
        get; set;
    }
}
