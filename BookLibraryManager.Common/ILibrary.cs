using System.Collections.ObjectModel;

namespace BookLibraryManager.Common;

/// <summary>
/// Represents a library interface that provides functionalities to add, remove, sort, and display books.
/// </summary>
/// <author>YR 2025-01-09</author>
public interface ILibrary : ILibraryBookAdder, ILibraryBookRemover, ILibraryBookSorter, ILibraryShower, ILibraryBookLocator
{
    /// <summary>
    /// Gets or sets the unique identifier for the library.
    /// </summary>
    int Id
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the collection of books in the library.
    /// </summary>
    ObservableCollection<Book> BookList
    {
        get; set;
    }
}
