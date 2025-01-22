using System.Windows;
using BookLibraryManager.TestApp;
using BookLibraryManager.TestApp.ViewModel;

namespace AppBookManager;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
/// <author>YR 2025-01-09</author>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        gridMainView.DataContext = new MainView();
    }
}