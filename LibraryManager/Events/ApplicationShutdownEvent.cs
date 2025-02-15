namespace LibraryManager.Events;

/// <summary>
/// Application shutdown event.
/// </summary>
/// <author>YR 2025-02-14</author>
public sealed class ApplicationShutdownEvent : PubSubEvent<ApplicationShutdownEventArgs>
{
}

/// <summary>
/// Provides data for the <see cref="ApplicationShutdownEvent"/> event.
/// </summary>
/// <author>YR 2025-02-14</author>
public sealed class ApplicationShutdownEventArgs : EventArgs
{
}
