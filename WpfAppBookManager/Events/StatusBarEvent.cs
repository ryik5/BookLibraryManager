
namespace BookLibraryManager.DemoApp.Events;

/// <summary>
/// Provides data for the StatusBar Event.
/// </summary>
/// /// <author>YR 2025-01-24</author>
public class StatusBarEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the message to be displayed in the status bar.
    /// </summary>
    public string Message
    {
        get; set;
    }

    /// <summary>
    /// Gets or sets the kind of status bar should be updated.
    /// </summary>
    public StatusBarKindEnum StatusBarKind
    {
        get; set;
    }
}

/// <summary>
/// Represents an event that updates the status bar.
/// </summary>
public class StatusBarEvent : PubSubEvent<StatusBarEventArgs>
{
}

/// <summary>
/// Specifies the kind of status bar to be updated.
/// </summary>
public enum StatusBarKindEnum
{
    /// <summary>
    /// The main window status bar.
    /// </summary>
    MainWindow,

    /// <summary>
    /// The find books status bar.
    /// </summary>
    FindBooks
}
