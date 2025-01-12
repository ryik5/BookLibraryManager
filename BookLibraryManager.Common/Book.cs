namespace BookLibraryManager.Common;

/// <summary>
/// Represents a book.
/// </summary>
/// <author>YR 2025-01-09</author>
[Serializable]
public class Book
{
    /// <summary>
    /// Gets or sets the unique identifier for the book.
    /// </summary>
    public required int Id
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the author of the book.
    /// </summary>
    public required string Author
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the title of the book.
    /// </summary>
    public required string Title
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the number of pages in the book.
    /// </summary>
    public required int PageNumber
    {
        get;
        set;
    }

    /// <summary>
    /// Returns a string that represents the current book.
    /// </summary>
    /// <returns>A string that contains the author, title, and page number of the book.</returns>
    public override string ToString()
    {
        return $"{Author}-{Title}-{PageNumber}";
    }
}
