using LibraryManager.Models;

namespace LibraryManager.Events;

/// <summary>
/// Represents an event that updates the status bar.
/// </summary>
/// <author>YR 2025-01-24</author>
public class StatusBarEvent : PubSubEvent<StatusBarEventArgs>
{
}

/// <summary>
/// Provides data for the StatusBar Event.
/// </summary>
/// <author>YR 2025-01-24</author>
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
    public EWindowKind WindowKind
    {
        get; set;
    }

    public EInfoKind InfoKind
    {
        get; set;
    }
}
