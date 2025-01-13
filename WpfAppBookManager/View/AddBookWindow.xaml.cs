using System.Windows;

namespace BookLibraryManager.TestApp.View;
/// <summary>
/// Interaction logic for AddBookWindow.xaml
/// </summary>
public partial class AddBookWindow : Window
{
    public AddBookWindow()
    {
        InitializeComponent();

        CloseWindow.WinObject = this;
    }
}
