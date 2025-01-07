namespace BookLibraryManager.Models;

public interface IBookListLoadable
{
    ILibrary LoadLibrary(string pathToFile);
}
