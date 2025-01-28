using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace BookLibraryManager.Common;

[XmlInclude(typeof(Book))]
[XmlInclude(typeof(MediaData))]
[XmlInclude(typeof(LibraryBookManagerModel))]
[Serializable]
[XmlRoot("Library")]
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
    /// Library name.
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    private string _name;

    /// <summary>
    /// Short description of the book.
    /// </summary>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
    private string _description;

    /// <summary>
    /// Total numbers of book in the library
    /// </summary>
    public int TotalBooks => BookList.Count;

    /// <summary>
    /// Gets or sets the collection of books in the library.
    /// </summary>
    [XmlArray]
    public ObservableCollection<Book> BookList
    {
        get => _bookList;
        set => SetProperty(ref _bookList, value);
    }
    private ObservableCollection<Book> _bookList;
}
