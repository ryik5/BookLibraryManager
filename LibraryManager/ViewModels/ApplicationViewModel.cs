using System.Windows.Input;
using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Utils;

namespace LibraryManager.ViewModels;

/// <summary>
/// The view model for the application (start point).
/// </summary>
/// <author>YR 2025-02-02</author>
public class ApplicationViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the ApplicationViewModel class.
    /// </summary>
    public ApplicationViewModel()
    {
        _library = new Library();

        _libraryManager = new LibraryManagerModel(_library);
        _bookManager = new BookManagerModel(_library);

        PageViewModels = new Dictionary<string, IViewModelPageable>
        {
            { "Library", new LibraryViewModel(_libraryManager) },
            { "Books", new BooksViewModel(_bookManager) },
            { "Find Books", new FindBookViewModel(_bookManager) },
            { "Tools", new ToolsViewModel() }, 
            { "Debug", new DebugViewModel() },
            { "About", new AboutViewModel() }
        };

        StatusBar = new StatusBarViewModel(_library);

        ChangePageCommand = new DelegateCommand<string>(Navigate);


        Navigate("Library"); // Default page
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
        if (pageName != null && TryGetPage(pageName, out var viewModel))
        {
            foreach (var kv in PageViewModels.Where(p => p.Value.IsChecked == true))
                kv.Value.IsChecked = false;

            CurrentViewModel = viewModel;
            CurrentViewModel.IsChecked = true;
            MessageHandler.SendToStatusBar($"Switched to '{pageName}' page", EInfoKind.DebugMessage);
        }
    }

    private bool TryGetPage(string pageName, out IViewModelPageable viewModel) => PageViewModels.TryGetValue(pageName, out viewModel);

    #endregion

    #region private fields
    private IViewModelPageable _currentViewModel;
    private readonly ILibraryManageable _libraryManager;
    private readonly IBookManageable _bookManager;
    private readonly ILibrary _library;
    private StatusBarViewModel _statusBar;
    #endregion
}
