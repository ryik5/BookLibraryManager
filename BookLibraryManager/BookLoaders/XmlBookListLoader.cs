using System.IO;
using System.Xml.Serialization;
using BookLibraryManager.Common;

namespace BookLibraryManager;
public class XmlBookListLoader : IBookListLoadable
{
    public ILibrary LoadLibrary(string filePath)
    {
        var serializer = new XmlSerializer(typeof(LibraryAbstract));
        object? deserializedBook;

        using var fileStream = new FileStream(filePath, FileMode.Open);
        deserializedBook = serializer.Deserialize(fileStream);

        return deserializedBook as ILibrary;
    }
}
