namespace LibraryManager.Models;

/// <summary>
/// Represents a status bar message with a name and tooltip.
/// </summary>
/// <author>YR 2025-01-24</author>
public class StatusBarMessage : BindableBase
{
    /// <summary>
    /// Represents the text of message for the status bar.
    /// </summary>
    public string MessageText
    {
        get => _messageText;
        set => SetProperty(ref _messageText, value);
    }

    /// <summary>
    /// Represents the tooltip of the status bar message.
    /// </summary>
    public string ToolTip
    {
        get => _toolTip;
        set => SetProperty(ref _toolTip, value);
    }

    public override string ToString() => MessageText;


    private string _messageText;
    private string _toolTip;
}
