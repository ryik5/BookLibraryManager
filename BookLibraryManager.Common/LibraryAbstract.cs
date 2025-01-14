using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BookLibraryManager.Common;

[XmlInclude(typeof(LibraryManagerModel))]
[Serializable]
/// <summary>
/// Represents an abstract base class for a library.
/// </summary>
/// <author>YR 2025-01-09</author>
public abstract class LibraryAbstract : BindableBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the library.
    /// </summary>
    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }
    private int _id;

    /// <summary>
    /// Gets or sets the collection of books in the library.
    /// </summary>
    public ObservableCollection<Book> BookList
    {
        get => _bookList;
        set => SetProperty(ref _bookList, value);
    }
    private ObservableCollection<Book> _bookList;
}
