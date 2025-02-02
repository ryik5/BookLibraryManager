using System.Windows;
using System.Windows.Input;
using BookLibraryManager.Common;
using LibraryManager.Models;

namespace LibraryManager.ViewModels
{
    /// <author>YR 2025-02-02</author>
    public class ApplicationViewModel : BindableBase
    {
        public ApplicationViewModel()
        {
            _libraryManager = new LibraryBookManagerModel();

            PageViewModels = new Dictionary<string, BindableBase>
            {
                { "Main", new MainViewModel(_libraryManager) }, //ButtonName(Content), ViewModel
                { "Find Book", new FindBookViewModel(_libraryManager) },
                { "About", new AboutViewModel() }
            };
            StatusBar = new StatusBarModel();

            ChangePageCommand = new DelegateCommand<string>(Navigate);

            if (PageViewModels.TryGetValue("Home", out var viewModel)) // Default page
                CurrentViewModel = viewModel;
        }


        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public Dictionary<string, BindableBase> PageViewModels
        {
            get;
        }

        public ICommand ChangePageCommand
        {
            get;
        }

        public StatusBarModel StatusBar
        {
            get => _statusBar;
            set => SetProperty(ref _statusBar, value);
        }
        private StatusBarModel _statusBar;



        private void Navigate(string pageName)
        {
            if (pageName != null && PageViewModels.TryGetValue(pageName, out var viewModel))
            {
                CurrentViewModel = viewModel;
            }
        }


        private BindableBase _currentViewModel;
        private readonly LibraryBookManagerModel _libraryManager;
    }
}
