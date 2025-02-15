namespace LibraryManager;

public static class Constants
{
    #region General
    public const string LIBRARY_MANAGER = "LibraryManager";
    public const string LIBRARY_MANAGER_PIPE_SERVER = "LibraryManagerPipeServer";
    #endregion

    #region ToolsViewModel
    public const string TOOLS = "Tools";
    #endregion

    #region StatusBarViewModel
    /// <summary>
    /// "Total books in the library"
    /// </summary>
    public static string TotalBookInLibrary(int? books = null) => books is null ? "Total books in the library" : $"Total books in the library: {books}";
    #endregion

    #region LibraryViewModel
    public const string LIBRARY = "Library";
    #endregion

    #region FindBookViewModel
    public const string FIND_BOOKS = "Find Books";
    #endregion

    #region DebugViewModel
    public const string DEBUG = "Debug";
    #endregion

    #region BooksViewModel
    public const string BOOKS = "Books";
    #endregion

    #region AboutViewModel
    public const string ABOUT = "About";
    #endregion

    #region CreatorBookDetailsViewModel
    public const string LOAD_CONTENT = "Load content";
    public const string ADD_BOOK = "Add Book";
    #endregion

}
