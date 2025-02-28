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
internal sealed class StatusBarViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the StatusBarViewModel class.
    /// Subscribes to the StatusBarEvent and sets up the status bar items.
    /// </summary>
    /// <param name="library">The library instance.</param>
    public StatusBarViewModel(ILibrary library)
    {
        _library = library;
        _library.LibraryIdChanged += HandleLibraryIdChanged;
        _token = App.EventAggregator.GetEvent<StatusBarEvent>().Subscribe(HandleStatusBarEvent);

        TotalBooksText.ToolTip = Constants.TOTAL_BOOKS_IN_LIBRARY;

        UpdateSysInfo();
        UpdateLibraryInfo();

        SelectedInfoItem.Content = TotalBooksText.MessageText;
        ToolTip = TotalBooksText.ToolTip;
    }

    public void Dispose()
    {
        App.EventAggregator.GetEvent<StatusBarEvent>().Unsubscribe(_token);
    }


    #region Properties
    /// <summary>
    /// Displays the common info messages within the stack.
    /// First line.
    /// </summary>
    public string TextInfoText1
    {
        get => _textInfoText1;
        set => SetProperty(ref _textInfoText1, value);
    }
    /// <summary>
    /// Displays the common info messages within the stack.
    /// Second line.
    /// </summary>
    public string TextInfoText2
    {
        get => _textInfoText2;
        set => SetProperty(ref _textInfoText2, value);
    }
    /// <summary>
    /// Displays the common info messages within the stack.
    /// Third line.
    /// </summary>
    public string TextInfoText3
    {
        get => _textInfoText3;
        set => SetProperty(ref _textInfoText3, value);
    }

    /// <summary>
    /// Displays the total number of the books in the library in the stack of <see cref="InfoList"/>
    /// </summary>
    public DisplayMessageModel TotalBooksText
    {
        get => _totalBooksText;
        set => SetProperty(ref _totalBooksText, value);
    }
    /// <summary>
    /// Displays the text info for the application version in the stack of <see cref="InfoList"/>.
    /// </summary>
    public DisplayMessageModel VersionText
    {
        get => _versionText;
        set => SetProperty(ref _versionText, value);
    }
    /// <summary>
    /// Displays the text info for the library information in the stack of <see cref="InfoList"/>.
    /// </summary>
    public DisplayMessageModel LibraryInfoText
    {
        get => _libraryInfoText;
        set => SetProperty(ref _libraryInfoText, value);
    }

    /// <summary>
    /// Gets or sets the currently shown info item in the combo box that selected between
    /// <see cref="TotalBooksText"/>, <see cref="VersionText"/>, anf <see cref="LibraryInfoText"/>.
    /// </summary>
    public ComboBoxItem SelectedInfoItem
    {
        get => _selectedInfoItem;
        set
        {
            if (SetProperty(ref _selectedInfoItem, value))
                ToolTip = value?.Tag;
        }
    }
    /// <summary>
    /// Gets or sets the tool tip text for the currently selected info item.
    /// <see cref="SelectedInfoItem"/></summary>
    public object ToolTip
    {
        get => _toolTip;
        set => SetProperty(ref _toolTip, value);
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
                UpdateStackTextInfoText(e.Message);
                break;
            case EInfoKind.DebugMessage:
                UpdateLibraryInfo();
                break;
        }

        /// <summary>
        /// Returns string 'total books in the library' or 'total books in the library: {books}'
        /// </summary>
        string TotalBooksInLibrary(int? books = null) => books is null
               ? Constants.TOTAL_BOOKS_IN_LIBRARY
               : $"{Constants.TOTAL_BOOKS_IN_LIBRARY}: {books}";
    }

    /// <summary>
    /// Updates the stack TextInfoText by shifting the existing text to the right and setting the new text at the top.
    /// </summary>
    private void UpdateStackTextInfoText(string msg)
    {
        TextInfoText3 = TextInfoText2;
        TextInfoText2 = TextInfoText1;
        TextInfoText1 = msg;
    }

    /// <summary>
    /// Updates the system information displayed in the status bar.
    /// </summary>
    private void UpdateSysInfo()
    {
        var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
        VersionText.MessageText = $"{versionInfo.CompanyName}, b.{Assembly.GetExecutingAssembly().GetName().Version}";
        VersionText.ToolTip = $"App. Info:{Environment.NewLine}Name:'{versionInfo.ProductName}'{Environment.NewLine}Company:'{versionInfo.CompanyName}'{Environment.NewLine}Build:'{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}.{versionInfo.FilePrivatePart}'";
    }

    /// <summary>
    /// Updates the library information displayed in the status bar.
    /// </summary>
    private void UpdateLibraryInfo()
    {
        LibraryInfoText.MessageText = $"{_library.Id}";
        LibraryInfoText.ToolTip = $"Library information.{Environment.NewLine}ID:'{_library.Id}'{Environment.NewLine}Name:'{_library.Name}'{Environment.NewLine}Description:'{_library.Description}'";
    }

    /// <summary>
    /// Handles the LibraryIdChanged event by updating the status bar.
    /// </summary>
    private void HandleLibraryIdChanged(object? sender, EventArgs e)
    {
        HandleStatusBarEvent(new StatusBarEventArgs()
        {
            Message = _library.TotalBooks.ToString(),
            InfoKind = EInfoKind.TotalBooks
        });

        UpdateLibraryInfo();
    }
    #endregion


    #region Fields
    private string _textInfoText1 = string.Empty;
    private string _textInfoText2 = string.Empty;
    private string _textInfoText3 = string.Empty;
    private DisplayMessageModel _totalBooksText = new();
    private DisplayMessageModel _versionText = new();
    private DisplayMessageModel _libraryInfoText = new();
    private ComboBoxItem _selectedInfoItem = new();
    private object _toolTip;
    private readonly SubscriptionToken _token;
    private readonly ILibrary _library;
    #endregion
}
