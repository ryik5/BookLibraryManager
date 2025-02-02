using System.Windows;
using LibraryManager.ViewModels;
using LibraryManager.Views;

namespace LibraryManager;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
/// <author>YR 2025-01-09</author>
public partial class App : Application
{
    public static readonly IEventAggregator EventAggregator = new EventAggregator();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var app = new ApplicationView();
        var context = new ApplicationViewModel();
        app.DataContext = context;
        app.Show();
    }
}

