using AppBookManager;
using BookLibraryManager.DemoApp.Events;

namespace BookLibraryManager.DemoApp.ViewModel;

/// <author>YR 2025-01-27</author>
public class MessageHandler
{
    /// <summary>
    /// Sends a message to the status bar.
    /// </summary>
    /// <param name="msg">The message to send.</param>
    public static void SendToStatusBar(StatusBarKindEnum statusBarKind, string msg)
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Publish(new StatusBarEventArgs
        {
            Message = msg,
            StatusBarKind = statusBarKind
        });
    }
}
