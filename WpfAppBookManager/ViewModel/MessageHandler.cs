﻿using AppBookManager;
using BookLibraryManager.DemoApp.Events;

namespace BookLibraryManager.DemoApp.ViewModel;

/// <author>YR 2025-01-27</author>
public class MessageHandler
{
    /// <summary>
    /// Sends a message to the status bar.
    /// </summary>
    /// <param name="msg">The message to send.</param>
    public static void SendToStatusBar(EWindowKind statusBarKind, EInfoKind infoKind, string msg)
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Publish(new StatusBarEventArgs
        {
            WindowKind = statusBarKind,
            InfoKind = infoKind,
            Message = msg,
        });
    }

    /// <summary>
    /// Sends a message to the status bar.
    /// </summary>
    /// <param name="msg">The message to send.</param>
    public static void SendToStatusBar(EInfoKind infoKind, string msg)
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Publish(new StatusBarEventArgs
        {
            InfoKind = infoKind,
            Message = msg,
        });
    }

    /// <summary>
    /// Sends a message to the status bar.
    /// </summary>
    /// <param name="msg">The message to send.</param>
    public static void SendToStatusBar(EWindowKind statusBarKind, string msg)
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Publish(new StatusBarEventArgs
        {
            WindowKind = statusBarKind,
            InfoKind = EInfoKind.CommonMessage,
            Message = msg,
        });
    }
}
