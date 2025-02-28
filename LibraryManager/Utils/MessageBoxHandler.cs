using System.Windows;
using LibraryManager.Models;
using LibraryManager.Properties;
using LibraryManager.Views;

namespace LibraryManager.Utils;

/// <summary>
/// A class for handling message box operations.
/// </summary>
/// <author>YR 2025-02-17</author>
internal sealed class MessageBoxHandler
{
    /// <summary>
    /// Shows a message box with the specified message and OK button.
    /// </summary>
    public void Show(string message)
    {
        SetDefaultState(EMessageBoxButtonsViewSelector.Ok);
        MessageBlock = message;
        _window = new MessageBoxWindow() { DataContext = this };
        _window.ShowDialog();
        MessageHandler.PublishMessage(message);
    }

    /// <summary>
    /// Displays a message box with the specified message, prompts for input string data, 
    /// and includes OK and Cancel buttons, with an optional title.
    /// </summary>
    /// <param name="message">The message to be displayed in the message box.</param>
    /// <param name="title">The title of the message box, or null if no title is desired.</param>
    public void ShowInput(string message, string title)
    {
        SetDefaultState(EMessageBoxButtonsViewSelector.OkCancel);
        MessageBlock = message;
        InputStringVisibility = Visibility.Visible;

        WindowTitle = title;
        _window = new MessageBoxWindow() { DataContext = this };
        _window.ShowDialog();
        MessageHandler.PublishMessage($"{title}: {message}");
    }

    /// <summary>
    /// Displays a message box with the specified title and message, using only OK button.
    /// </summary>
    public void Show(string title, string message)
    {
        SetDefaultState(EMessageBoxButtonsViewSelector.Ok);
        MessageBlock = message;
        WindowTitle = title;
        _window = new MessageBoxWindow() { DataContext = this };
        _window.ShowDialog();
        MessageHandler.PublishMessage($"{title}: {message}");
    }
    /// <summary>
    /// Displays a message box with the specified title, message, using the button view selector.
    /// </summary>
    public void Show(string title, string message, EMessageBoxButtonsViewSelector buttonsViewSelector)
    {
        SetDefaultState(buttonsViewSelector);
        NoButtonName = "No";
        MessageBlock = message;
        WindowTitle = title;
        _window = new MessageBoxWindow() { DataContext = this };
        _window.ShowDialog();
        MessageHandler.PublishMessage($"{title}: {message}");
    }
    /// <summary>
    /// Displays a message box with the specified title, message, and buttons.
    /// </summary>
    public void Show(string title, string message, EMessageBoxButtonsViewSelector buttonsViewSelector, string executeButtonName, string noButtonName = "No")
    {
        SetDefaultState(buttonsViewSelector);
        ExecuteButtonName = executeButtonName;
        NoButtonName = noButtonName;
        MessageBlock = message;
        WindowTitle = title;
        _window = new MessageBoxWindow() { DataContext = this };
        _window.ShowDialog();
        MessageHandler.PublishMessage($"{title}: {message}");
    }


    public string WindowTitle
    {
        get; set;
    }

    public string ExecuteButtonName
    {
        get; set;
    }
    public DelegateCommand<Window> ExecuteCommand
    {
        get; private set;
    }
    public string NoButtonName
    {
        get; set;
    }

    public DelegateCommand<Window> NoCommand
    {
        get; private set;
    }
    public Visibility NoButtonVisibility
    {
        get; set;
    }

    public DelegateCommand<Window> CancelCommand
    {
        get; private set;
    }
    public Visibility CancelButtonVisibility
    {
        get; set;
    }

    public string MessageBlock
    {
        get; set;
    }
    public double MessageFontSize
    {
        get; private set;
    }


    public string InputString
    {
        get; set;
    }
    public Visibility InputStringVisibility
    {
        get; set;
    }


    public EDialogResult DialogResult
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

    private void SetDefaultState(EMessageBoxButtonsViewSelector buttonsViewSelector)
    {
        SetControlsVisibility(buttonsViewSelector);
        ExecuteButtonName = "OK";
        NoButtonName = "No";
        WindowTitle = "General information";
        MessageFontSize = Settings.Default.MessageBox_FontSize;
        ExecuteCommand = new DelegateCommand<Window>(window => SetDialogResult(window, EDialogResult.YesButton));
        NoCommand = new DelegateCommand<Window>(window => SetDialogResult(window, EDialogResult.NoButton));
        CancelCommand = new DelegateCommand<Window>(window => SetDialogResult(window, EDialogResult.CancelButton));
        DialogResult = EDialogResult.NoButton;
    }

    private void SetControlsVisibility(EMessageBoxButtonsViewSelector result)
    {
        switch (result)
        {
            case EMessageBoxButtonsViewSelector.Yes:
            case EMessageBoxButtonsViewSelector.Ok:
                NoButtonVisibility = Visibility.Collapsed;
                CancelButtonVisibility = Visibility.Collapsed;
                break;
            case EMessageBoxButtonsViewSelector.YesNoCancel:
            case EMessageBoxButtonsViewSelector.OkNoCancel:
                NoButtonVisibility = Visibility.Visible;
                CancelButtonVisibility = Visibility.Visible;
                break;
            case EMessageBoxButtonsViewSelector.YesNo:
            case EMessageBoxButtonsViewSelector.OkNo:
                NoButtonVisibility = Visibility.Visible;
                CancelButtonVisibility = Visibility.Collapsed;
                break;
            case EMessageBoxButtonsViewSelector.YesCancel:
            case EMessageBoxButtonsViewSelector.OkCancel:
                NoButtonVisibility = Visibility.Collapsed;
                CancelButtonVisibility = Visibility.Collapsed;
                break;
        }
        InputStringVisibility = Visibility.Collapsed;
    }

    private void SetDialogResult(Window window, EDialogResult result)
    {
        DialogResult = result;
        CloseWindow(window);
    }



    private MessageBoxWindow _window;
}
