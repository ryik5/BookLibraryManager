using LibraryManager.Events;
using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <author>YR 2025-02-14</author>
internal sealed class ToolsViewModel : BindableBase, IViewModelPageable
{
    public ToolsViewModel(SettingsModel settings)
    {
        _settings = new SettingsViewModel(settings);
        RaisePropertyChanged(nameof(Settings));

        App.EventAggregator.GetEvent<ApplicationShutdownEvent>().Subscribe(HandleApplicationShutdownEvent);
    }


    #region Properties
    public string Name => Constants.TOOLS;

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }

    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetProperty(ref _isEnabled, value);
    }

    public SettingsViewModel Settings
    {
        get => _settings;
        set => SetProperty(ref _settings, value);
    }
    #endregion



    private void HandleApplicationShutdownEvent(ApplicationShutdownEventArgs e)
    {
        Settings.SaveSettings();
    }

    #region Fields
    private bool _isChecked;
    private bool _isEnabled = true;
    private SettingsViewModel _settings;
    #endregion
}
