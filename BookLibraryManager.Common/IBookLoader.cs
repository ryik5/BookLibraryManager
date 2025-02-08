using System.Windows.Media.Imaging;

namespace BookLibraryManager.Common;

/// <author>YR 2025-01-09</author>
public interface IBookLoader : ILoadable
{
    bool TryLoadBook(string pathToBook, out Book? book);

    bool TryLoadImage(string pathToData, out BitmapImage? image);

    event EventHandler<ActionFinishedEventArgs> LoadingFinished;
}