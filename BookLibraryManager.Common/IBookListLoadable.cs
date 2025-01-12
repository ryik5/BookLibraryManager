namespace BookLibraryManager.Common;

/// <summary>
/// Defines an interface for loading a library by a specified path.
/// </summary>
/// <author>YR 2025-01-09</author>
public interface IBookListLoadable
{
    /// <summary>
    /// Loads a library from the given path.
    /// </summary>
    /// <param name="pathToLibrary">The path to the library</param>
    /// <returns>An instance of <see cref="ILibrary"/>representing the loaded library.</returns>
    ILibrary LoadLibrary(string pathToLibrary);
}
