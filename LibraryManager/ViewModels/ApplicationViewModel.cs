using System.Windows.Input;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;

namespace LibraryManager.ViewModels;

/// <summary>
/// The view model for the application (start point).
/// </summary>
/// <author>YR 2025-02-02</author>
internal sealed class ApplicationViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the ApplicationViewModel class.
    /// </summary>
    public ApplicationViewModel()
    {
        _settings = new SettingsModel();
        var toolsVM = new ToolsViewModel(_settings);

        _library = new Library();

        _libraryManager = new LibraryManagerModel(_library);
        _bookManager = new BookManagerModel(_library);

        var libraryVM = new LibraryViewModel(_libraryManager);
        var booksVM = new BooksViewModel(_bookManager);
        var finderVM = new FindBookViewModel(_bookManager, _settings);
        var debugVM = new DebugViewModel();
        var aboutVM = new AboutViewModel();
        PageViewModels = new Dictionary<string, IViewModelPageable>
        {
            { libraryVM.Name, libraryVM },
            { booksVM.Name, booksVM },
            { finderVM.Name, finderVM},
            { toolsVM.Name,  toolsVM},
            {debugVM.Name, debugVM},
            { aboutVM.Name, aboutVM }
        };

        StatusBar = new StatusBarViewModel(_library);

        ChangePageCommand = new DelegateCommand<string>(Navigate);

        Navigate(libraryVM.Name); // Default page
    }

    #region Properties
    /// <summary>
    /// Gets or sets the current IViewModelPageable model to display on the view.
    /// </summary>
    public IViewModelPageable CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    /// <summary>
    /// Gets the page view models involved into the displaying data.
    /// </summary>
    public Dictionary<string, IViewModelPageable> PageViewModels
    {
        get;
    }

    /// <summary>
    /// Gets or sets the status bar of the application.
    /// </summary>
    public StatusBarViewModel StatusBar
    {
        get => _statusBar;
        set => SetProperty(ref _statusBar, value);
    }
    #endregion


    #region Commands
    /// <summary>
    /// Gets the change IViewModelPageable command.
    /// </summary>
    public ICommand ChangePageCommand
    {
        get;
    }
    #endregion


    #region private methods
    /// <summary>
    /// Navigates to the specified page.
    /// </summary>
    /// <param name="pageName">The name of the page to navigate to.</param>
    private void Navigate(string pageName)
    {
        if (pageName != null && PageViewModels.TryGetValue(pageName, out var viewModel))
        {
            foreach (var kv in PageViewModels.Where(p => p.Value.IsChecked == true))
                kv.Value.IsChecked = false;

            CurrentViewModel = viewModel;
            CurrentViewModel.IsChecked = true;
            MessageHandler.SendToStatusBar($"Switched to '{pageName}' page", EInfoKind.DebugMessage);
        }
    }
    #endregion

    #region private fields
    private readonly SettingsModel _settings;
    private readonly ILibrary _library;
    private IViewModelPageable _currentViewModel;
    private readonly ILibraryManageable _libraryManager;
    private readonly IBookManageable _bookManager;
    private StatusBarViewModel _statusBar;
    #endregion
}
