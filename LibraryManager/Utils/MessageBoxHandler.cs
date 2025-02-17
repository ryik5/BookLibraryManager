using System.Windows;
using LibraryManager.Models;
using LibraryManager.Views;

namespace LibraryManager.Utils;

/// <summary>
/// A class for handling message box operations.
/// </summary>
/// <author>YR 2025-02-17</author>
public class MessageBoxHandler
{
    /// <summary>
    /// Shows a message box with the specified message and OK button.
    /// </summary>
    public void Show(string message)
    {
        SetDefaultState(MessageBoxButtonsViewSelector.Ok);
        MessageBlock = message;
        _window = new MessageBoxWindow() { DataContext = this };
        _window.ShowDialog();
    }
    /// <summary>
    /// Shows a message box with the specified title and message, using the default OK button.
    /// </summary>
    public void Show(string title, string message)
    {
        SetDefaultState(MessageBoxButtonsViewSelector.Ok);
        MessageBlock = message;
        _window = new MessageBoxWindow() { Title = title, DataContext = this };
        _window.ShowDialog();
    }
    /// <summary>
    /// Shows a message box with the specified title, message, and button view selector.
    /// </summary>
    public void Show(string title, string message, MessageBoxButtonsViewSelector buttonsViewSelector)
    {
        SetDefaultState(buttonsViewSelector);
        NoButtonName = "No";
        MessageBlock = message;
        _window = new MessageBoxWindow() { Title = title, DataContext = this };
        _window.ShowDialog();
    }
    /// <summary>
    /// Shows a message box with the specified title, message, and buttons.
    /// </summary>
    public void Show(string title, string message, MessageBoxButtonsViewSelector buttonsViewSelector, string executeButtonName, string noButtonName = "No")
    {
        SetDefaultState(buttonsViewSelector);
        ExecuteButtonName = executeButtonName;
        NoButtonName = noButtonName;
        MessageBlock = message;
        _window = new MessageBoxWindow() { Title = title, DataContext = this };
        _window.ShowDialog();
    }


    public string ExecuteButtonName
    {
        get; set;
    }
    public string NoButtonName
    {
        get; set;
    }
    public Visibility NoButtonVisibility
    {
        get; set;
    }
    public Visibility CancelButtonVisibility
    {
        get; set;
    }
    public string MessageBlock
    {
        get; set;
    }
    public DelegateCommand<Window> ExecuteCommand
    {
        get; private set;
    }
    public DelegateCommand<Window> NoCommand
    {
        get; private set;
    }
    public DelegateCommand<Window> CancelCommand
    {
        get; private set;
    }
    public Models.DialogResult DialogResult
    {
        get; private set;
    }

    /// <summary>
    /// Closes the specified window.
    /// </summary>
    /// <param name="window">The window to be closed.</param>
    private void CloseWindow(Window window)
    {
        window?.Close();
        _window?.Close();
    }

    private void SetDefaultState(MessageBoxButtonsViewSelector buttonsViewSelector)
    {
        SetDialogResultVisibility(buttonsViewSelector);
        ExecuteButtonName = "OK";
        NoButtonName = "No";
        ExecuteCommand = new DelegateCommand<Window>(window => SetDialogResult(window, Models.DialogResult.YesButton));
        NoCommand = new DelegateCommand<Window>(window => SetDialogResult(window, Models.DialogResult.NoButton));
        CancelCommand = new DelegateCommand<Window>(window => SetDialogResult(window, Models.DialogResult.CancelButton));
        DialogResult = Models.DialogResult.NoButton;
    }

    private void SetDialogResultVisibility(MessageBoxButtonsViewSelector result)
    {
        switch (result)
        {
            case MessageBoxButtonsViewSelector.Yes:
            case MessageBoxButtonsViewSelector.Ok:
                NoButtonVisibility = Visibility.Collapsed;
                CancelButtonVisibility = Visibility.Collapsed;
                break;
            case MessageBoxButtonsViewSelector.YesNoCancel:
            case MessageBoxButtonsViewSelector.OkNoCancel:
                NoButtonVisibility = Visibility.Visible;
                CancelButtonVisibility = Visibility.Visible;
                break;
            case MessageBoxButtonsViewSelector.YesNo:
            case MessageBoxButtonsViewSelector.OkNo:
                NoButtonVisibility = Visibility.Visible;
                CancelButtonVisibility = Visibility.Collapsed;
                break;
            case MessageBoxButtonsViewSelector.YesCancel:
            case MessageBoxButtonsViewSelector.OkCancel:
                NoButtonVisibility = Visibility.Collapsed;
                CancelButtonVisibility = Visibility.Collapsed;
                break;
        }
    }

    private void SetDialogResult(Window window, Models.DialogResult result)
    {
        DialogResult = result;
        CloseWindow(window);
    }




    private MessageBoxWindow _window;
}

