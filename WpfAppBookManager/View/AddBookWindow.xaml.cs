using System.Windows;

namespace BookLibraryManager.TestApp.View;
/// <summary>
/// Interaction logic for AddBookWindow.xaml
/// </summary>
/// <author>YR 2025-01-09</author>
public partial class AddBookWindow : Window
{
    public AddBookWindow()
    {
        InitializeComponent();

        CloseWindow.WinObject = this;
    }
}
