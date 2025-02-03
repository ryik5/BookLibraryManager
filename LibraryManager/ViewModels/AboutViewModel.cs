namespace LibraryManager.ViewModels
{
    /// <author>YR 2025-02-02</author>
    public class AboutViewModel : BindableBase
    {
        private string _message = $"Developer: @YR\nDesigner: @Ila Yavorska\nVersion:{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string Name => "About";
    }
}
