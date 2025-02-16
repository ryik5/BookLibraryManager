namespace LibraryManager.Utils;

/// <summary>
/// A utility class responsible for loading data asynchronously.
/// </summary>
/// <author>YR 2025-01-26</author>
internal sealed class Handler
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
            MessageHandler.SendDebugMessage($"An error occurred: {ex.Message}");

            return default;
        }
    }
}
