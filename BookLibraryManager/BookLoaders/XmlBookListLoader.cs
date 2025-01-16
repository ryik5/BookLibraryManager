using System.IO;
using System.Xml.Serialization;
using BookLibraryManager.Common;

namespace BookLibraryManager;

/// <summary>
/// Loader of the library from XML file stored on a local disk.
/// </summary>
/// <author>YR 2025-01-09</author>
public class XmlBookListLoader : IBookListLoadable
{
    /// <summary>
    /// Loads the library from the specified XML file.
    /// </summary>
    /// <param name="pathToLibrary">The path to the XML file containing the library data.</param>
    /// <param name="library">The loaded library instance.</param>
    /// <returns>True if the library was successfully loaded; otherwise, false.</returns>
    public bool LoadLibrary(string pathToLibrary, out ILibrary? library)
    {
        bool result;
        library = null;
        try
        {
            var serializer = new XmlSerializer(typeof(LibraryAbstract));

            using var fileStream = new FileStream(pathToLibrary, FileMode.Open);
            library = serializer.Deserialize(fileStream) as ILibrary;
            result = true;
        }
        catch { result = false; }
        return result;
    }
}
