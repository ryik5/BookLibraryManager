using System.Windows;

namespace AppBookManager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/// <author>YR 2025-01-09</author>
public partial class MainWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the MainWindow class.
    /// Sets the DataContext of gridMainView to a new instance of MainViewModel.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }
}
