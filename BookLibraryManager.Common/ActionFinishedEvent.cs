namespace BookLibraryManager.Common;

public class ActionFinishedEventArgs : EventArgs
{
    public bool IsFinished { get; set; }

    public string Message { get; set; }
}

public class ActionFinishedEven : PubSubEvent<ActionFinishedEventArgs>
{
}