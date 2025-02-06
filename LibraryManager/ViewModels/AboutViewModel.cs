using System.Diagnostics;
using System.Reflection;
using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <author>YR 2025-02-02</author>
public class AboutViewModel : BindableBase, IViewModelPageable
{

    #region Properties
    public string Header => _headMessage;

    public string Footer
    {
        get => _footMessage;
        set => SetProperty(ref _footMessage, value);
    }

    public string Name => "About";

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }
    #endregion

    #region Fields
    private string _footMessage = $"Developer: @YR\nDesigner: @Ila Yavorska\nVersion: {Assembly.GetExecutingAssembly().GetName().Version}";
    private readonly string _headMessage = $"App. {FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).CompanyName}, b.{Assembly.GetExecutingAssembly().GetName().Version}";
    private bool _isChecked;
    #endregion
}
