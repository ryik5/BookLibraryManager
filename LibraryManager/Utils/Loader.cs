using System.IO.Pipes;

namespace LibraryManager.Utils;

/// <summary>
/// A utility class responsible for loading data asynchronously.
/// </summary>
/// <author>YR 2025-01-26</author>
internal class Loader
{
    /// <summary>
    /// Loads data asynchronously using the provided action.
    /// </summary>
    /// <param name="loadData">The action to load the data.</param>
    public void LoadDataAsync(Action loadData)
    {
        Task.Run(async () => await TryLoadByOneStream(loadData));
    }

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
    /// Loads data asynchronously using the provided function and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of data to load.</typeparam>
    /// <param name="loadData">The function to load the data.</param>
    /// <returns>The loaded data.</returns>
    public async Task<T> LoadDataAsync<T>(Func<T> loadData) where T : class
    {
        return await TryLoadDataTaskByOneStream(loadData);
    }

    /// <summary>
    /// Attempts to load data by one stream, ensuring that only one instance can load data at a time, and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of data to load.</typeparam>
    /// <param name="loadData">The function to load the data.</param>
    /// <returns>The loaded data.</returns>
    private async Task<T> TryLoadDataTaskByOneStream<T>(Func<T> loadData) where T : class
    {
        T bitmap = null;
        using (var mutex = new Mutex(true, _mutexName, out var onlyInstance))
        {
            if (onlyInstance)
            {
                try
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    var cancellationToken = cancellationTokenSource.Token;
                    bitmap = await LoadDataTask(cancellationToken, loadData);

                    cancellationTokenSource.Cancel();
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }
        return bitmap;
    }

    /// <summary>
    /// Loads data asynchronously using the provided cancellation token and function.
    /// </summary>
    /// <typeparam name="T">The type of data to load.</typeparam>
    /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
    /// <param name="loadData">The function to load the data.</param>
    /// <returns>The loaded data.</returns>
    private async Task<T> LoadDataTask<T>(CancellationToken cancellationToken, Func<T> loadData)
    {
        T? data = default;
        using (var pipeServer = new NamedPipeServerStream(_namedPipeName))
        {
            var IsLoading = true;
            while (!cancellationToken.IsCancellationRequested && IsLoading)
            {
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    data = loadData.Invoke(); // Load content
                    IsLoading = false;
                });
            }
        }
        return data;
    }


    private const string _mutexName = "BookManager";
    private const string _namedPipeName = "BookManagerPipeServer";
}
