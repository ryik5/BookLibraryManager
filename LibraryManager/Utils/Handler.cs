using System.IO.Pipes;
using LibraryManager.Models;

namespace LibraryManager.Utils;

/// <summary>
/// A utility class responsible for loading data asynchronously.
/// </summary>
/// <author>YR 2025-01-26</author>
internal class Handler
{
    public static async Task<T?> ExecuteTaskAsync<T>(Func<T> task) where T : class
    {
        try
        {
            return await Task.Run(task);
        }
        catch (Exception ex)
        {
            // Handle the exception
            MessageHandler.SendToStatusBar($"An error occurred: {ex.Message}", EInfoKind.DebugMessage);

            return default;
        }
    }



    /// <summary>
    /// Loads data asynchronously using the provided action.
    /// </summary>
    /// <param name="loadData">The action to load the data.</param>
    private void LoadDataAsync(Action loadData)=> ExecuteTaskAsync(()=> Task.FromResult(TryLoadByOneStream(loadData)));

    /// <summary>
    /// Attempts to load data by one stream, ensuring that only one instance can load data at a time.
    /// </summary>
    /// <param name="loadData">The action to load the data.</param>
    private async Task TryLoadByOneStream(Action loadData)
    {
        using (var mutex = new Mutex(true, _mutexName, out var onlyInstance))
        {
            if (onlyInstance)
            {
                try
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    var cancellationToken = cancellationTokenSource.Token;
                    await Task.Run(() => LoadContentTask(cancellationToken, loadData), cancellationToken);

                    cancellationTokenSource.Cancel();
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }

    /// <summary>
    /// Loads content asynchronously using the provided cancellation token and action.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <param name="loadData">The action to load the content.</param>
    private async Task LoadContentTask(CancellationToken cancellationToken, Action loadData)
    {
        using (var pipeServer = new NamedPipeServerStream(_namedPipeName))
        {
            var IsLoading = true;
            while (!cancellationToken.IsCancellationRequested && IsLoading)
            {
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    loadData.Invoke(); // Load content
                    IsLoading = false;
                });
            }
        }
    }

    /// <summary>
    /// Attempts to invoke action by one stream, ensuring that only one instance can do it at a time.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    public async Task TryInvokeActionAsync(Action action)
    {
        using (var mutex = new Mutex(true, _mutexName, out var onlyInstance))
        {
            if (onlyInstance)
            {
                try
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    var cancellationToken = cancellationTokenSource.Token;
                    await InvokeActionTask(cancellationToken, action);

                    cancellationTokenSource.Cancel();
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }


    /// <summary>
    /// Loads data asynchronously using the provided cancellation token and function.
    /// </summary>
    /// <typeparam name="T">The type of data to load.</typeparam>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <param name="action">The function to load the data.</param>
    private async Task InvokeActionTask(CancellationToken cancellationToken, Action action)
    {
        using (var pipeServer = new NamedPipeServerStream(_namedPipeName))
        {
            var IsLoading = true;
            while (!cancellationToken.IsCancellationRequested && IsLoading)
            {
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    action.Invoke(); // Invoke action
                    IsLoading = false;
                });
            }
        }
    }


    private const string _mutexName = "LibraryManager";
    private const string _namedPipeName = "LibraryManagerPipeServer";
}
