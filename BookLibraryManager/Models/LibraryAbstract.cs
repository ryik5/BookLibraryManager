using System.Xml.Serialization;

namespace BookLibraryManager.Models;

[XmlInclude(typeof(LibraryModel))]
[Serializable]
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
