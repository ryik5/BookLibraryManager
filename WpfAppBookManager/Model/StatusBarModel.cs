using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using AppBookManager;
using BookLibraryManager.DemoApp.Events;

namespace BookLibraryManager.DemoApp.Model;

/*    
   private SubscriptionToken _statusBarEventToken;
   private void Subscribe()
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Subscribe(OnGridEvents);

    // 2. _statusBarEventToken = App.EventAggregator.GetEvent<StatusBarEvent>().Subscribe(OnGridEvents);
    }

    private void Unsubscribe()
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Unsubscribe(OnGridEvents);

      //2.  App.EventAggregator.GetEvent<StatusBarEvent>().Unsubscribe(_statusBarEventToken);
    }
*/

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
    public StatusBarModel(StatusBarKindEnum statusBarKind)
    {
        _statusBarKind = statusBarKind;

        App.EventAggregator.GetEvent<StatusBarEvent>().Subscribe(HandleStatusBarEvent);

        StatusBarInformer.Content = StatusBarText;
        BarItems.Add(StatusBarInformer);
    }

    /// <summary>
    /// Handles the StatusBarEvent by updating the status bar text if the event's status bar kind matches.
    /// </summary>
    /// <param name="e">The event arguments containing the message and status bar kind.</param>
    private void HandleStatusBarEvent(StatusBarEventArgs e)
    {
        if (_statusBarKind == e.StatusBarKind)
            StatusBarText.Text = e.Message;
    }

    /// <summary>
    /// Gets or sets the collection of status bar items.
    /// </summary>
    public ObservableCollection<StatusBarItem> BarItems
    {
        get => _barItems;
        set => SetProperty(ref _barItems, value);
    }
    private ObservableCollection<StatusBarItem> _barItems = new();

    /// <summary>
    /// Gets or sets the status bar informer item.
    /// </summary>
    public StatusBarItem StatusBarInformer
    {
        get => _statusBarInformer;
        set => SetProperty(ref _statusBarInformer, value);
    }
    private StatusBarItem _statusBarInformer = new()
    {
        VerticalContentAlignment = VerticalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center
    };

    /// <summary>
    /// Gets or sets the text block used to display the status bar message.
    /// </summary>
    public TextBlock StatusBarText
    {
        get => _statusBarMessage;
        set => SetProperty(ref _statusBarMessage, value);
    }
    private TextBlock _statusBarMessage = new()
    {
        FontSize = 10,
        Padding = new Thickness(0),
        Margin = new Thickness(5, 0, 5, 0),
        VerticalAlignment = VerticalAlignment.Center,
        TextTrimming = TextTrimming.CharacterEllipsis

    };

    private readonly StatusBarKindEnum _statusBarKind;
}
