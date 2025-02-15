using LibraryManager.Events;
using LibraryManager.Models;

namespace LibraryManager.Utils;

/// <author>YR 2025-01-27</author>
internal sealed class MessageHandler
{
    /// <summary>
    /// Sends a message to the status bar.
    /// </summary>
    /// <param name="msg">The message to send.</param>
    public static void SendToStatusBar(string msg, EInfoKind infoKind = EInfoKind.CommonMessage)
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Publish(new StatusBarEventArgs
        {
            InfoKind = infoKind,
            Message = msg,
        });
    }
}

