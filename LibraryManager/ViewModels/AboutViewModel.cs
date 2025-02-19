using System.Diagnostics;
using System.Reflection;
using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <summary>
/// Represents a view model for the About page.
/// </summary>
/// <author>YR 2025-02-02</author>
internal sealed class AboutViewModel : BindableBase, IViewModelPageable
{
    public AboutViewModel()
    {
        versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

        _header = $"App.{versionInfo.ProductName}, Author:'{versionInfo.CompanyName}', build:'{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}.{versionInfo.FilePrivatePart}'";
        _footer = $"Developer: @YR{Environment.NewLine}Designer: @Ila Yavorska";
    }


    #region Properties
    public string Name => Constants.ABOUT;

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }

    /// <summary>
    /// Gets the Header of the About Page.
    /// </summary>
    public string Header => _header;

    /// <summary>
    /// Gets the Footer of the About Page.
    /// </summary>
    public string Footer
    {
        get => _footer;
        set => SetProperty(ref _footer, value);
    }
    #endregion

    #region Fields
    private FileVersionInfo? versionInfo = null;
    private string _footer;
    private string _header;
    private bool _isChecked;
    #endregion
}
