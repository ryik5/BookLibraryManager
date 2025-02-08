using System.Diagnostics;
using System.Reflection;
using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <author>YR 2025-02-02</author>
public class AboutViewModel : BindableBase, IViewModelPageable
{
    public AboutViewModel()
    {
        versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

        _headMessage = $"App.:{versionInfo.ProductName}, Company:'{versionInfo.CompanyName}', version:'{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}.{versionInfo.FilePrivatePart}'";
        _footMessage = $"Developer: @YR{Environment.NewLine}Designer: @Ila Yavorska";
    }


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

    public bool IsEnabled
    {
        get => _isEnabled;
        set => SetProperty(ref _isEnabled, value);
    }
    #endregion

    #region Fields


    private FileVersionInfo? versionInfo = null;
    private string _footMessage;
    private string _headMessage;
    private bool _isChecked;
    private bool _isEnabled = true;
    #endregion
}
