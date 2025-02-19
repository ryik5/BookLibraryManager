using LibraryManager.Events;
using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <summary>
/// View model for managing application tools.
/// </summary>
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

    public SettingsViewModel Settings
    {
        get => _settings;
        set => SetProperty(ref _settings, value);
    }
    #endregion

    /// <summary>
    /// Handles the ApplicationShutdownEvent by saving the application settings.
    /// </summary>
    /// <param name="e">The event arguments containing information about the application shutdown.</param>
    private void HandleApplicationShutdownEvent(ApplicationShutdownEventArgs e)
    {
        Settings.SaveSettings();
    }

    #region Fields
    private bool _isChecked;
    private SettingsViewModel _settings;
    #endregion
}
