namespace BookLibraryManager.Common;

public interface IBookListLoadable
{
    ILibrary LoadLibrary(string pathToLibrary);
}
