namespace BookLibraryManager.Common;

/// <author>YR 2025-01-26</author>
public class ActionFinishedEventArgs : EventArgs
{
    public bool IsFinished { get; set; }

    public string Message { get; set; }
}

/// <author>YR 2025-01-26</author>
public class ActionFinishedEvent : PubSubEvent<ActionFinishedEventArgs>
{
}