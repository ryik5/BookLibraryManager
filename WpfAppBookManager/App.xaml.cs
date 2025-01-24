using System.Windows;

namespace AppBookManager;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
/// <author>YR 2025-01-09</author>
public partial class App : Application
{
    public static readonly IEventAggregator EventAggregator = new EventAggregator();

}
