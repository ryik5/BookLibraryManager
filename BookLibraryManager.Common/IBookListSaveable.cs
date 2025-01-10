namespace BookLibraryManager.Common;

public interface IBookListSaveable
{
    bool SaveLibrary(ILibrary library, string selectedFolder);
}
