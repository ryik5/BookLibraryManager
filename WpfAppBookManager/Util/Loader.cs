using System.IO.Pipes;
using AppBookManager;

namespace BookLibraryManager.DemoApp.Util;

/// <author>YR 2025-01-26</author>
public class Loader
{
    public Loader()
    {

    }

    public void LoadDataAsync(Action loadData)
    {
        Task.Run(async () => await TryLoadByOneStream(loadData));
    }

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
            else
            {
                // skip loading
            }
        }
    }

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

    public async Task<T> LoadDataAsync<T>(Func<T> loadData) where T : class
    {
        return await TryLoadDataTaskByOneStream(loadData);
    }

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
            else
            {
                // skip loading
            }
        }
        return bitmap;
    }

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
