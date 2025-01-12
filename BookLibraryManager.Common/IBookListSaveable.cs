namespace BookLibraryManager.Common;

/// <author>YR 2025-01-09</author>
public interface IBookListSaveable
{
    bool SaveLibrary(ILibrary library, string selectedFolder);
}
