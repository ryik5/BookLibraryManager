namespace BookLibraryManager.Models;

public interface IBookListSaveable
{
    bool SaveLibrary(ILibrary library, string selectedFolder);
}
