namespace LibraryManager.Models;

/// <summary>
/// A log message with a specified log level, message, and timestamp.
/// </summary>
/// <author>YR 2025-02-05</author>
public sealed class LogMessage
{
    /// <summary>
    /// Gets the log level of the message.
    /// </summary>
    public LogLevel Level
    {
        get;
    }

    /// <summary>
    /// Gets the message text.
    /// </summary>
    public string Message
    {
        get;
    }

    /// <summary>
    /// Gets the timestamp when the log message was created.
    /// </summary>
    public DateTime Timestamp
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogMessage"/> class with the specified log level and message.
    /// </summary>
    /// <param name="level">The log level of the message.</param>
    /// <param name="message">The message text.</param>
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
