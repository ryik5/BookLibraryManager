using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
using BookLibraryManager.Common;
using LibraryManager.Events;
using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <summary>
/// The StatusBarModel class is responsible for managing the status bar in the application.
/// It subscribes to the StatusBarEvent to update the status bar text based on the event data.
/// The class provides properties to manage the collection of status bar items, the status bar informer item,
/// and the text block used to display the status bar message.
/// </summary>
/// <author>YR 2025-01-24</author>
public sealed class StatusBarViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the StatusBarModel class.
    /// Subscribes to the StatusBarEvent and sets up the status bar items.
    /// </summary>
    /// <param name="statusBarKind">The kind of status bar to be used.</param>
    public StatusBarViewModel(ILibrary library)
    {
        _library = library;
        _token = App.EventAggregator.GetEvent<StatusBarEvent>().Subscribe(HandleStatusBarEvent);

        TotalBooksText.ToolTip = Constants.TOTAL_BOOKS_IN_LIBRARY;
        InfoList.Add(TotalBooksText);

        UpdateSysInfo();
        UpdateLibraryInfo();

        InfoList.Add(VersionText);
        InfoList.Add(LibraryInfoText);

        SelectedInfoItem.Content = TotalBooksText.MessageText;
        ToolTip = TotalBooksText.ToolTip;
    }

    public void Dispose()
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Unsubscribe(_token);
    }


    #region Properties
    /// <summary>
    /// Displays the common info messages.
    /// </summary>
    public string TextInfoText1
    {
        get => _textInfoText1;
        set => SetProperty(ref _textInfoText1, value);
    }
    public string TextInfoText2
    {
        get => _textInfoText2;
        set => SetProperty(ref _textInfoText2, value);
    }
    public string TextInfoText3
    {
        get => _textInfoText3;
        set => SetProperty(ref _textInfoText3, value);
    }

    /// <summary>
    /// Displays the total number of the books in the library.
    /// </summary>
    public MessageWithToolTip TotalBooksText
    {
        get => _totalBooksText;
        set => SetProperty(ref _totalBooksText, value);
    }

    public MessageWithToolTip VersionText
    {
        get => _versionText;
        set => SetProperty(ref _versionText, value);
    }

    public MessageWithToolTip LibraryInfoText
    {
        get => _libraryInfoText;
        set => SetProperty(ref _libraryInfoText, value);
    }

    public ComboBoxItem SelectedInfoItem
    {
        get => _selectedInfoItem;
        set
        {
            if (SetProperty(ref _selectedInfoItem, value))
                ToolTip = value?.Tag;
        }
    }

    public object ToolTip
    {
        get => _toolTip;
        set => SetProperty(ref _toolTip, value);
    }

    public ObservableCollection<MessageWithToolTip> InfoList
    {
        get => _infoList;
        set => SetProperty(ref _infoList, value);
    }
    #endregion

    #region private methods
    /// <summary>
    /// Handles the StatusBarEvent by updating the status bar text if the event's status bar kind matches.
    /// </summary>
    /// <param name="e">The event arguments containing the message and status bar kind.</param>
    private void HandleStatusBarEvent(StatusBarEventArgs e)
    {
        switch (e.InfoKind)
        {
            case EInfoKind.TotalBooks:
                TotalBooksText.MessageText = e.Message;
                TotalBooksText.ToolTip = TotalBooksInLibrary(_library.TotalBooks);
                RaisePropertyChanged(nameof(TotalBooksText));
                break;
            case EInfoKind.CommonMessage:
                SetTextInfoText(e.Message);
                break;
            case EInfoKind.DebugMessage:
                UpdateSysInfo();
                UpdateLibraryInfo();
                break;
        }
    }

    private void SetTextInfoText(string msg)
    {
        TextInfoText3 = TextInfoText2;
        TextInfoText2 = TextInfoText1;
        TextInfoText1 = msg;
    }

    private void UpdateSysInfo()
    {
        var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
        VersionText.MessageText = $"{versionInfo.CompanyName}, b.{Assembly.GetExecutingAssembly().GetName().Version}";
        VersionText.ToolTip = $"App. Info:{Environment.NewLine}Name:'{versionInfo.ProductName}'{Environment.NewLine}Company:'{versionInfo.CompanyName}'{Environment.NewLine}Build:'{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}.{versionInfo.FilePrivatePart}'";
    }

    private void UpdateLibraryInfo()
    {
        LibraryInfoText.MessageText = $"Lib: {_library.Id}";
        LibraryInfoText.ToolTip = $"Library information.{Environment.NewLine}ID:'{_library.Id}'{Environment.NewLine}Name:'{_library.Name}'{Environment.NewLine}Description:'{_library.Description}'";
    }
    /// <summary>
    /// Returns string 'total books in the library' or 'total books in the library: {books}'
    /// </summary>
    private static string TotalBooksInLibrary(int? books = null) => books is null
        ? Constants.TOTAL_BOOKS_IN_LIBRARY
        : $"{Constants.TOTAL_BOOKS_IN_LIBRARY}: {books}";
    #endregion


    #region Fields
    private string _textInfoText1 = string.Empty;
    private string _textInfoText2 = string.Empty;
    private string _textInfoText3 = string.Empty;
    private MessageWithToolTip _totalBooksText = new();
    private MessageWithToolTip _versionText = new();
    private MessageWithToolTip _libraryInfoText = new();
    private ComboBoxItem _selectedInfoItem = new();
    private object _toolTip;
    private ObservableCollection<MessageWithToolTip> _infoList = new();
    private readonly SubscriptionToken _token;
    private readonly ILibrary _library;
    #endregion
}
