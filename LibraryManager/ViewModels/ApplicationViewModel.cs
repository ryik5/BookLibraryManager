using System.Windows.Input;
using BookLibraryManager.Common;
using LibraryManager.Models;

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
        _libraryManager = new LibraryBookManagerModel();

        PageViewModels = new Dictionary<string, IViewModelPageable>
        {
            { "Main", new MainViewModel(_libraryManager) }, //CheckBox content, ViewModel
            { "Find Book", new FindBookViewModel(_libraryManager) },
            { "About", new AboutViewModel() }
        };
        StatusBar = new StatusBarModel();


        ChangePageCommand = new DelegateCommand<string>(Navigate);

        Navigate("Main"); // Default page
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
    public StatusBarModel StatusBar
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
        }
    }
    #endregion

    #region private fields
    private IViewModelPageable _currentViewModel;
    private readonly LibraryBookManagerModel _libraryManager;
    private StatusBarModel _statusBar;
    #endregion
}
