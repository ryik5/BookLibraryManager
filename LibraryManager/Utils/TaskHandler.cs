namespace LibraryManager.Utils;

/// <summary>
/// A utility class responsible for loading data asynchronously.
/// </summary>
/// <author>YR 2025-01-26</author>
internal sealed class TaskHandler
{
    public static async Task<T?> TryExecuteTaskAsync<T>(Func<T> task) where T : class
    {
        try
        {
            return await Task.Run(task);
        }
        catch (Exception ex)
        {
            // Handle the exception
            MessageHandler.PublishDebugMessage($"An error occurred: {ex.Message}");

            return default;
        }
    }
}
