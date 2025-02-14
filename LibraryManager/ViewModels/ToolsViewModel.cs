using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <author>YR 2025-02-14</author>
public class ToolsViewModel : BindableBase, IViewModelPageable
{

    #region Properties
    public string Name => "Tools";

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
    #endregion

    #region Fields
    private bool _isChecked;
    private bool _isEnabled = true;
    #endregion
}
