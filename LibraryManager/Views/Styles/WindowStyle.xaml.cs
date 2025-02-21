﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using LibraryManager.Events;

namespace LibraryManager.Views;

#region  WindowStyle. Start block 
/// <summary>
/// Interaction logic for WindowStyle.xaml
/// </summary>
public partial class WindowStyle
{
    void WindowLoaded(object sender, RoutedEventArgs e)
    {
        var w = ((Window)sender);

        UpdateWindowSize(w);
        w.StateChanged += WindowStateChanged;
    }

    void WindowStateChanged(object sender, EventArgs e)
    {
        var w = ((Window)sender);
        UpdateWindowSize(w);
    }

    void UpdateWindowSize(Window w)
    {
        var containerBorder = (Border)w.Template.FindName("PART_Container", w);

        if (w.WindowState == WindowState.Maximized)
        {
            w.Height = SystemParameters.MaximumWindowTrackHeight - 20;
            w.Width = SystemParameters.MaximumWindowTrackWidth - 20;

            containerBorder.Padding = new Thickness(
                SystemParameters.WorkArea.Left, //+ 7,
                SystemParameters.WorkArea.Top, //+ 7,
                (SystemParameters.PrimaryScreenWidth - SystemParameters.WorkArea.Right), //+ 7,
                (SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Bottom) + 5); // +5);
        }
        else
        {
            containerBorder.Padding = new Thickness(0);
        }
    }

    void CloseButtonClick(object sender, RoutedEventArgs e)
    {
        App.EventAggregator.GetEvent<ApplicationShutdownEvent>().Publish(new ApplicationShutdownEventArgs());

        sender.ForWindowFromTemplate(w => SystemCommands.CloseWindow(w));
    }

    void MaxButtonClick(object sender, RoutedEventArgs e)
    {
        sender.ForWindowFromTemplate(w =>
        {
            if (w.WindowState == WindowState.Maximized)
                SystemCommands.RestoreWindow(w);
            else
                SystemCommands.MaximizeWindow(w);
        });
    }

    void TitleMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        sender.ForWindowFromTemplate(w =>
        {
            if (w.WindowState != WindowState.Maximized)
                w.WindowState = WindowState.Maximized;
        });
    }
}

internal static class LocalExtensions
{
    public static void ForWindowFromTemplate(this object templateFrameworkElement, Action<Window> action)
    {
        if (((FrameworkElement)templateFrameworkElement).TemplatedParent is Window window)
            action(window);
    }

    public static IntPtr GetWindowHandle(this Window window)
    {
        var helper = new WindowInteropHelper(window);
        return helper.Handle;
    }
}
#endregion
