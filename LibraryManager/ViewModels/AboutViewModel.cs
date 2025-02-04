using LibraryManager.Models;

namespace LibraryManager.ViewModels;

/// <author>YR 2025-02-02</author>
public class AboutViewModel : BindableBase, IViewModelPageable
{
    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }


    #region Properties
    public string Name => "About";

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }
    #endregion

    #region Fields
    private string _message = $"Developer: @YR\nDesigner: @Ila Yavorska\nVersion: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
    private bool _isChecked;
    #endregion
}
