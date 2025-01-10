namespace BookLibraryManager.Common;

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

    public override string ToString()
    {
        return $"{Author}-{Title}-{PageNumber}";
    }
}
