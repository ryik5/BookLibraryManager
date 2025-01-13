using System.Windows;

namespace BookLibraryManager.TestApp;
public static class CloseWindow
{
    public static Window WinObject;

    public static void CloseParent()
    {
        try
        {
            WinObject?.Close();
        }
        catch
        {
        }
    }
}

