using System.Collections.ObjectModel;

namespace BookLibraryManager.Common;

/// <summary>
/// Represents a library interface.
/// </summary>
/// <author>YR 2025-01-09</author>
public interface ILibrary : ILoadable
{
    /// <summary>
    /// Unique identifier for the library.
    /// </summary>
    int Id
    {
        get; set;
    }

    /// <summary>
    /// Occurs when the library ID changes.
    /// </summary>
    event EventHandler<EventArgs> LibraryIdChanged;

    /// <summary>
    /// Library name.
    /// </summary>
    string Name
    {
        get; set;
    }

    /// <summary>
    /// Description of the library.
    /// </summary>
    string Description
    {
        get; set;
    }

    /// <summary>
    /// total number of books in the library.
    /// </summary>
    int TotalBooks
    {
        get;
    }

    /// <summary>
    /// Gets or sets the collection of books in the library.
    /// </summary>
    ObservableCollection<Book> BookList
    {
        get; set;
    }
}
