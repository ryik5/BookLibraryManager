namespace LibraryManager.Utils;

public static class GeneralHandler
{
    #region StatusBarViewModel
    /// <summary>
    /// Returns string 'total books in the library' or 'total books in the library: {books}'
    /// </summary>
    public static string TotalBooksInLibrary(int? books = null) => books is null
        ? Constants.TOTAL_BOOKS_IN_LIBRARY 
        : $"{Constants.TOTAL_BOOKS_IN_LIBRARY}: {books}";
    #endregion

}
