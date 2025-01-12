using System.Xml.Serialization;

namespace BookLibraryManager.Common;

[XmlInclude(typeof(LibraryModel))]
[Serializable]
/// <summary>
/// Represents an abstract base class for a library.
/// </summary>
/// <author>YR 2025-01-09</author>
public abstract class LibraryAbstract
{
    /// <summary>
    /// Gets or sets the unique identifier for the library.
    /// </summary>
    public int Id
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the list of books in the library.
    /// </summary>
    public List<Book> BookList
    {
        get;
        set;
    }
}
