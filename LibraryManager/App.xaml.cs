using System.Windows;
using System.Windows.Media.Imaging;
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

        var app = new ApplicationView() { Icon = new BitmapImage(new Uri("pack://application:,,,/Properties/Resources/library.ico", UriKind.RelativeOrAbsolute)) };

        var context = new ApplicationViewModel();
        app.DataContext = context;
        app.Show();
    }
}

