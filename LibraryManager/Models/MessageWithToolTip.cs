namespace LibraryManager.Models;

/// <summary>
/// Represents a two string properties class with a message <see cref="MessageText"/> and tooltip to message <see cref="ToolTip"/>.
/// </summary>
/// <author>YR 2025-01-24</author>
public sealed class MessageWithToolTip : BindableBase
{
    /// <summary>
    /// Represents the text of message
    /// </summary>
    public string MessageText
    {
        get => _messageText;
        set => SetProperty(ref _messageText, value);
    }

    /// <summary>
    /// Represents the tooltip for the <see cref="MessageText"/> .
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
