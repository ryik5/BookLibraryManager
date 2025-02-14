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

    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetProperty(ref _isEnabled, value);
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
                DebugTextLog += CreateLogEntry(LogLevel.Info, $"Total books in the library: {e.Message}");
                break;
            default:
                DebugTextLog += CreateLogEntry(LogLevel.Info, e.Message);
                break;
        }

    }

    /// <summary>
    /// Logs a message with the specified level.
    /// </summary>
    /// <param name="level">The log level (e.g. Debug, Info, Warn, Error).</param>
    /// <param name="message">The message to log.</param>
    private string CreateLogEntry(LogLevel level, string message) => $"{new LogMessage(level, message)}{Environment.NewLine}";

    #region private fields
    private string _debugTextLog = string.Empty;
    private bool _isChecked;
    private bool _isEnabled = true;
    #endregion
}
