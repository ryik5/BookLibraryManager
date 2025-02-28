using System.Text;
using LibraryManager.Events;
using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <summary>
/// View model for the debug page, responsible for handling log messages and text font size.
/// </summary>
/// <author>YR 2025-02-05</author>
internal sealed class DebugViewModel : BindableBase, IViewModelPageable
{
    public DebugViewModel(SettingsModel settings)
    {
        _settings = settings;

        App.EventAggregator.GetEvent<StatusBarEvent>().Subscribe(HandleStatusBarEvent);
    }

    #region Properties
    public string Name => Constants.DEBUG;

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }

    /// <summary>
    /// Gets or sets the log builder.
    /// </summary>
    /// <value>The log builder.</value>
    public StringBuilder LogBuilder
    {
        get => _logBuilder;
        private set => SetProperty(ref _logBuilder, value);
    }

    /// <summary>
    /// Gets or sets the text's font size.
    /// </summary>
    /// <value>The log builder.</value>
    public double TextFontSize
    {
        get => _settings.Debug_TextFontSize;
        private set => SetProperty(ref _settings.Debug_TextFontSize, value);
    }
    #endregion


    #region private methods
    /// <summary>
    /// Handles the StatusBarEvent by updating the status bar text if the event's status bar kind matches.
    /// </summary>
    /// <param name="e">The event arguments containing the message and status bar kind.</param>
    private void HandleStatusBarEvent(StatusBarEventArgs e)
    {
        switch (e.InfoKind)
        {
            case EInfoKind.TotalBooks:
                UpdateTextLog(CreateLogEntry(ELogLevel.Info, $"Total books in the library: {e.Message}"));
                break;
            default:
                UpdateTextLog(CreateLogEntry(ELogLevel.Info, e.Message));
                break;
        }

    }

    /// <summary>
    /// Updates the text log by inserting the specified message at the beginning.
    /// </summary>
    /// <param name="msg">The message to insert into the log.</param>
    private void UpdateTextLog(string msg)
    {
        LogBuilder.Insert(0, msg);
    }

    /// <summary>
    /// Logs a message with the specified level.
    /// </summary>
    /// <param name="level">The log level (e.g. Debug, Info, Warn, Error).</param>
    /// <param name="message">The message to log.</param>
    private string CreateLogEntry(ELogLevel level, string message) => $"{new LoggedEventModel(level, message)}{Environment.NewLine}";
    #endregion


    #region private fields
    private readonly SettingsModel _settings;
    private bool _isChecked;
    private StringBuilder _logBuilder = new();
    #endregion
}
