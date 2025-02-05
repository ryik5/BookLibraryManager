using LibraryManager.Events;

namespace LibraryManager.Models;

/// <summary>
/// The StatusBarModel class is responsible for managing the status bar in the application.
/// It subscribes to the StatusBarEvent to update the status bar text based on the event data.
/// The class provides properties to manage the collection of status bar items, the status bar informer item,
/// and the text block used to display the status bar message.
/// </summary>
/// <author>YR 2025-01-24</author>
public class StatusBarModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the StatusBarModel class.
    /// Subscribes to the StatusBarEvent and sets up the status bar items.
    /// </summary>
    /// <param name="statusBarKind">The kind of status bar to be used.</param>
    public StatusBarModel()
    {
        _token = App.EventAggregator.GetEvent<StatusBarEvent>().Subscribe(HandleStatusBarEvent);
    }

    public void Dispose()
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Unsubscribe(_token);
    }


    #region Properties
    /// <summary>
    /// Displays the common info messages.
    /// </summary>
    public string TextInfoText
    {
        get => _textInfoText;
        set => SetProperty(ref _textInfoText, value);
    }

    /// <summary>
    /// Displays the total number of the books in the library.
    /// </summary>
    public string TotalPagesText
    {
        get => _totalPagesText;
        set => SetProperty(ref _totalPagesText, value);
    }
    #endregion


    /// <summary>
    /// Handles the StatusBarEvent by updating the status bar text if the event's status bar kind matches.
    /// </summary>
    /// <param name="e">The event arguments containing the message and status bar kind.</param>
    private void HandleStatusBarEvent(StatusBarEventArgs e)
    {
        switch (e.InfoKind)
        {
            case EInfoKind.TotalPages:
                TotalPagesText = e.Message;
                break;
            case EInfoKind.CommonMessage:
                TextInfoText = e.Message;
                break;
        }
    }


    #region Fields
    private string _textInfoText;
    private string _totalPagesText;
    private readonly SubscriptionToken _token;
    #endregion
}
