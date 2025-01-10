using System.IO;
using System.Xml.Serialization;
using BookLibraryManager.Common;

namespace BookLibraryManager;
public class XmlBookListSaver : IBookListSaveable
{
    public bool SaveLibrary(ILibrary library, string pathToFile)
    {

        using var fileStream = new FileStream(pathToFile, FileMode.Create);
        new XmlSerializer(typeof(LibraryAbstract)).Serialize(fileStream, library as LibraryAbstract);

        return true;
    }
}
