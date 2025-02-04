namespace LibraryManager.Models;

/// <author>YR 2025-02-04</author>
public interface IViewModelPageable
{
    string Name
    {
        get;
    }

    bool IsChecked
    {
        get; set;
    }

}