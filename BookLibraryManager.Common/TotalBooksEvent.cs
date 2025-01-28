namespace BookLibraryManager.Common;

/// <author>YR 2025-01-28</author>
public class TotalBooksEventArgs : EventArgs
{
    public int TotalBooks
    {
        get; set;
    }
}

/// <author>YR 2025-01-28</author>
public class TotalBooksEvent : PubSubEvent<TotalBooksEventArgs>
{
}