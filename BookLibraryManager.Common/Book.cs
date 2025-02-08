using System.Xml.Serialization;

namespace BookLibraryManager.Common;

/// <summary>
/// Represents a book.
/// </summary>
/// <author>YR 2025-01-09</author>
public class Book : BindableBase, ILoadable, ICloneable
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
    /// Gets or sets the number of total pages in the book.
    /// </summary>
    public required int TotalPages
    {
        get => _totalPages;
        set => SetProperty(ref _totalPages, value);
    }
    private int _totalPages;

    /// <summary>
    /// Gets or sets the publication date of the book.
    /// </summary>
    public int PublishDate
    {
        get => _publishDate;
        set => SetProperty(ref _publishDate, value);
    }
    private int _publishDate;

    /// <summary>
    /// Gets or sets the description of the book.
    /// </summary>
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
    private string _description;

    /// <summary>
    /// Gets or sets the genre of the book.
    /// </summary>
    public string Genre
    {
        get => _genre;
        set => SetProperty(ref _genre, value);
    }
    private string _genre;

    /// <summary>
    /// Gets or sets the ISBN of the book.
    /// </summary>
    public string ISBN
    {
        get => _isbn;
        set => SetProperty(ref _isbn, value);
    }
    private string _isbn;

    /// <summary>
    /// Gets or sets the media content of the book.
    /// </summary>
    [XmlElement("MediaData")]
    public MediaData Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }
    private MediaData _content;

    /// <summary>
    /// Returns a string that represents the current book.
    /// </summary>
    /// <returns>A string that contains the author, title, and total pages of the book.</returns>
    public override string ToString()
    {
        return $"Author:{Author}-Title:{Title}-Pages:{TotalPages}-Year:{PublishDate}";
    }

    /// <summary>
    /// Creates a deep copy of the current Book object.
    /// </summary>
    /// <returns>A new Book object that is a copy of the current object.</returns>
    public object Clone()
    {
        Book clone = new()
        {
            Id = Id,
            Author = Author,
            Title = Title,
            TotalPages = TotalPages,
            PublishDate = PublishDate,
            Description = Description,
            Content = Content is null ? null : new()
            {
                BmImage = Content.BmImage,
                ContentType = Content.ContentType,
                Image = Content.Image,
                Name = Content.Name,
                OriginalPath = Content.OriginalPath,
                PictureByteArray = Content.PictureByteArray
            },
            Genre = Genre,
            ISBN = ISBN
        };
        return clone;
    }
}
