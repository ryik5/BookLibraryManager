using System.IO;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using BookLibraryManager.XmlLibraryProvider;

namespace BookLibraryManager.Common;

/// <summary>
/// Loader of the library from XML file stored on a local disk.
/// </summary>
/// <author>YR 2025-01-09</author>
public class XmlLibraryLoader : ILibraryLoader
{
    /// <summary>
    /// Loads the library from the specified XML file.
    /// </summary>
    /// <param name="pathToLibrary">The path to the XML file containing the library data.</param>
    /// <param name="library">The loaded library instance.</param>
    /// <returns>True if the library was successfully loaded; otherwise, false.</returns>
    public bool TryLoadLibrary(string pathToLibrary, out ILibrary? library)
    {
        var result = false;
        library = null;
        var msg = string.Empty;
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = "Loading started", IsFinished = false });
        LibraryAbstract lib = null;
        try
        {
            lib = XMLObjectSerializer.Load<LibraryAbstract>(pathToLibrary);
            result = true;
            msg = "Library loaded";
        }
        catch (Exception ex)
        {
            result = false;
            msg = "Library was not loaded";
        }
        library = lib as ILibrary;

        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = msg, IsFinished = result });
        return result;
    }

    public bool TryLoadBook(string pathToBook, out Book? book)
    {
        var result = false;
        book = null;
        var msg = string.Empty;
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = "Loading started", IsFinished = false });

        try
        {
            var serializer = new XmlSerializer(typeof(Book));

            using var fileStream = new FileStream(pathToBook, FileMode.Open);
            book = serializer.Deserialize(fileStream) as Book;
            result = true;
            msg = "Book loaded";
        }
        catch
        {
            result = false;
            msg = "Book was not loaded";
        }
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = msg, IsFinished = result });

        return result;
    }

    public bool TryLoadImage(string pathToData, out BitmapImage? image)
    {
        var uri = new Uri(pathToData);
        var result = false;
        image = null;
        var msg = string.Empty;
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = "Loading started", IsFinished = false });

        try
        {
            image = new BitmapImage(uri);
            result = true;
            msg = "Image loaded";
        }
        catch
        {
            result = false;
            msg = "Image was not loaded";
        }
        LoadingFinished?.Invoke(this, new ActionFinishedEventArgs { Message = msg, IsFinished = result });

        return result;
    }

    public event EventHandler<ActionFinishedEventArgs> LoadingFinished;
}
