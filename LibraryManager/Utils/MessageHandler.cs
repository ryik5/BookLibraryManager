using LibraryManager.Events;
using LibraryManager.Models;

namespace LibraryManager.Utils;

/// <summary>
/// Handles sending messages to the status bar viewmodel.
/// </summary>
/// <author>YR 2025-01-27</author>
internal sealed class MessageHandler
{
    /// <summary>
    /// Publishes a message to the status bar with the specified info kind.
    /// </summary>
    /// <param name="msg">The message to publish.</param>
    /// <param name="infoKind">The type of information being published (default is CommonMessage).</param>
    public static void PublishMessage(string msg) => SendMsgToStatusBar(msg, EInfoKind.CommonMessage);

    /// <summary>
    /// Publishes a debug message to the status bar.
    /// </summary>
    /// <param name="msg">The debug message to publish.</param>
    public static void PublishDebugMessage(string msg) => SendMsgToStatusBar(msg, EInfoKind.DebugMessage);

    /// <summary>
    /// Publishes the total number of books in the library to the status bar.
    /// </summary>
    /// <param name="books">The total number of books in the library.</param>
    public static void PublishTotalBooksInLibrary(int books) => SendMsgToStatusBar($"{books}", EInfoKind.TotalBooks);

    /// <summary>
    /// Publishes a message to the status bar with the specified info kind.
    /// </summary>
    /// <param name="msg">The message to publish.</param>
    /// <param name="infoKind">The type of information being published (default is CommonMessage).</param>
    private static void SendMsgToStatusBar(string msg, EInfoKind infoKind)
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Publish(new StatusBarEventArgs
        {
            InfoKind = infoKind,
            Message = msg,
        });
    }
}
