using System.Windows.Input;
using BookLibraryManager.Common;
using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <author>YR 2025-02-02</author>
public class ApplicationViewModel : BindableBase
{
    public ApplicationViewModel()
    {
        _libraryManager = new LibraryBookManagerModel();

        PageViewModels = new Dictionary<string, IViewModelPageable>
        {
            { "Main", new MainViewModel(_libraryManager) }, //ButtonName(Content), ViewModel
            { "Find Book", new FindBookViewModel(_libraryManager) },
            { "About", new AboutViewModel() }
        };
        StatusBar = new StatusBarModel();


        ChangePageCommand = new DelegateCommand<string>(Navigate);

        Navigate("Main"); // Default page
    }


    #region Properties
    public IViewModelPageable CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public Dictionary<string, IViewModelPageable> PageViewModels
    {
        get;
    }

    public StatusBarModel StatusBar
    {
        get => _statusBar;
        set => SetProperty(ref _statusBar, value);
    }
    #endregion


    #region Commands
    public ICommand ChangePageCommand
    {
        get;
    }
    #endregion


    #region private methods
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
