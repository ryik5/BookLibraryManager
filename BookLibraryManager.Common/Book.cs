namespace BookLibraryManager.Common;

/// <summary>
/// Represents a book.
/// </summary>
/// <author>YR 2025-01-09</author>
[Serializable]
public class Book : BindableBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the book.
    /// </summary>
    public required int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }
    private int _id;

    /// <summary>
    /// Gets or sets the author of the book.
    /// </summary>
    public required string Author
    {
        get => _author;
        set => SetProperty(ref _author, value);
    }
    private string _author;

    /// <summary>
    /// Gets or sets the title of the book.
    /// </summary>
    public required string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
    private string _title;

    /// <summary>
    /// Gets or sets the number of pages in the book.
    /// </summary>
    public required int PageNumber
    {
        get => _pageNumber;
        set => SetProperty(ref _pageNumber, value);
    }
    private int _pageNumber;

    /// <summary>
    /// Returns a string that represents the current book.
    /// </summary>
    /// <returns>A string that contains the author, title, and page number of the book.</returns>
    public override string ToString()
    {
        return $"{Author}-{Title}-{PageNumber}";
    }
}
