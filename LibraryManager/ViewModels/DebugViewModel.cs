using LibraryManager.Events;
using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <author>YR 2025-02-05</author>
public class DebugViewModel : BindableBase, IViewModelPageable
{
    public DebugViewModel()
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Subscribe(HandleStatusBarEvent);
    }

    #region Properties
    /// <summary>
    /// Text of logging.
    /// </summary>
    public string DebugTextLog
    {
        get => _debugTextLog;
        set => SetProperty(ref _debugTextLog, value);
    }

    public string Name => "Debug";

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }
    #endregion


    /// <summary>
    /// Handles the StatusBarEvent by updating the status bar text if the event's status bar kind matches.
    /// </summary>
    /// <param name="e">The event arguments containing the message and status bar kind.</param>
    private void HandleStatusBarEvent(StatusBarEventArgs e)
    {
        switch (e.InfoKind)
        {
            case EInfoKind.TotalPages:
                DebugTextLog += MakeLogEntry(LogLevel.Info, $"Total books in the library: {e.Message}");
                break;
            default:
                DebugTextLog += MakeLogEntry(LogLevel.Info, e.Message);
                break;
        }

    }

    /// <summary>
    /// Logs a message with the specified level.
    /// </summary>
    /// <param name="level">The log level (e.g. Debug, Info, Warn, Error).</param>
    /// <param name="message">The message to log.</param>
    private string MakeLogEntry(LogLevel level, string message)
    {
        var logMessage = new LogMessage(level, message);
        return $"{logMessage}\n";
    }

    #region private fields
    private string _debugTextLog = string.Empty;
    private bool _isChecked;
    #endregion
}


/// <summary>
/// A log message.
/// </summary>
/// <author>YR 2025-02-05</author>
public class LogMessage
{
    public LogLevel Level
    {
        get;
    }
    public string Message
    {
        get;
    }
    public DateTime Timestamp
    {
        get;
    }

    public LogMessage(LogLevel level, string message)
    {
        Level = level;
        Message = message;
        Timestamp = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}";
    }
}

/// <summary>
/// The log level.
/// </summary>
/// <author>YR 2025-02-05</author>
public enum LogLevel
{
    Debug,
    Info,
    Warn,
    Error
}
