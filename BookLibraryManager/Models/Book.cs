namespace BookLibraryManager.Models;

[Serializable]
public class Book
{
    public required int Id
    {
        get;
        set;
    }

    public required string Author
    {
        get;
        set;
    }

    public required string Title
    {
        get;
        set;
    }

    public required int PageNumber
    {
        get;
        set;
    }
}
