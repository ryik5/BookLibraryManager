using System.Xml.Serialization;

namespace BookLibraryManager.Common;

[XmlInclude(typeof(LibraryModel))]
[Serializable]
/// <author>YR 2025-01-09</author>
public abstract class LibraryAbstract
{
    public int Id
    {
        get;
        set;
    }

    public List<Book> BookList
    {
        get;
        set;
    }
}
