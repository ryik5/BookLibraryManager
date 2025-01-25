using System.IO.Pipes;
using AppBookManager;

namespace BookLibraryManager.DemoApp.Util;

public class Loader
{
    public Loader()
    {

    }

    public async Task LoadDataAsync(Action loadData)
    {
        using (var mutex = new Mutex(true, _mutexName, out var onlyInstance))
        {
            if (onlyInstance)
            {
                try
                {
                    var cancellationTokenSource = new CancellationTokenSource();
                    var cancellationToken = cancellationTokenSource.Token;
                    await Task.Run(() => LoadContentAsync(cancellationToken, loadData), cancellationToken);

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



    private async Task LoadContentAsync(CancellationToken cancellationToken, Action loadData)
    {
        try
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
        catch (Exception exc)
        {
        }
    }

    private const string _mutexName = "BookManager";
    private const string _namedPipeName = "BookManagerPipeServer";
}
