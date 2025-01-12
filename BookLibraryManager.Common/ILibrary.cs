﻿namespace BookLibraryManager.Common;

/// <summary>
/// Represents a library interface that provides functionalities to add, remove, sort, and display books.
/// </summary>
/// <author>YR 2025-01-09</author>
public interface ILibrary : ILibraryAddBook, ILibraryRemoveBook, ILibrarySort, ILibraryShower
{
    /// <summary>
    /// Gets or sets the unique identifier for the library.
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// Gets or sets the list of books in the library.
    /// </summary>
    List<Book> BookList { get; set; }

    /// <summary>
    /// Creates a clone of the current library instance.
    /// </summary>
    /// <returns>A new instance of ILibrary that is a copy of the current instance.</returns>
    ILibrary Clone();
}
