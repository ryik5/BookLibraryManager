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
    /// Loads a library from the specified XML file path.
    /// </summary>
    /// <param name="filePath">The path to the XML file containing the library data.</param>
    /// <returns>An instance of <see cref="ILibrary"/> representing the loaded and deseriliazed library.</returns>
    public ILibrary LoadLibrary(string filePath)
    {
        var serializer = new XmlSerializer(typeof(LibraryAbstract));
        object? deserializedBook;

        using var fileStream = new FileStream(filePath, FileMode.Open);
        deserializedBook = serializer.Deserialize(fileStream);

        return deserializedBook as ILibrary;
    }
}
