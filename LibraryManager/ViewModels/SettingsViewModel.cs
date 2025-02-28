using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Properties;
using LibraryManager.Utils;

namespace LibraryManager.ViewModels;

/// <summary>
/// View model for managing application settings.
/// </summary>
/// <author>YR 2025-02-14</author>
internal sealed class SettingsViewModel : BindableBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
    /// </summary>
    /// <param name="settings">The settings model to use.</param>
    public SettingsViewModel(SettingsModel settings)
    {
        _settings = settings;
        LoadAllSettings();
    }

    /// <summary>
    /// Resets all application settings to their default values.
    /// </summary>
    public void ResetAllSettings()
    {
        Settings.Default.Properties.Cast<System.ComponentModel.PropertyDescriptor>().ToList().ForEach(p => p.ResetValue(Settings.Default));
    }

    /// <summary>
    /// Loads all application settings from the settings store.
    /// </summary>
    public void LoadAllSettings()
    {
        TryConvertStringToEnum(Settings.Default.FindBooks_LastSearchField, ref _settings.SearchField, SearchField);
        SearchOnFly = Settings.Default.FindBooks_SearchOnFly;
        Debug_TextFontSize = Settings.Default.Debug_TextFontSize;

        FirstSortBookProperty = Settings.Default.BooksView_FirstSortBookProperty;
        FirstSortProperty_ByDescend = Settings.Default.BooksView_FirstSortProperty_ByDescent;
        SecondSortBookProperty = Settings.Default.BooksView_SecondSortBookProperty;
        SecondSortProperty_ByDescend = Settings.Default.BooksView_SecondSortProperty_ByDescent;
        ThirdSortBookProperty = Settings.Default.BooksView_ThirdSortBookProperty;
        ThirdSortProperty_ByDescend = Settings.Default.BooksView_ThirdSortProperty_ByDescent;

        Book_MaxContentLength = Settings.Default.Book_MaxContentLength;
    }

    /// <summary>
    /// Saves all application settings to the settings store.
    /// </summary>
    public void SaveSettings()
    {
        Settings.Default.FindBooks_LastSearchField = _settings.SearchField.ToString();
        Settings.Default.FindBooks_SearchOnFly = _settings.SearchOnFly;
        Settings.Default.Debug_TextFontSize = _settings.Debug_TextFontSize;

        Settings.Default.BooksView_FirstSortBookProperty = _settings.FirstSortBookProperty;
        Settings.Default.BooksView_FirstSortProperty_ByDescent = _settings.FirstSortProperty_ByDescend;
        Settings.Default.BooksView_SecondSortBookProperty = _settings.SecondSortBookProperty;
        Settings.Default.BooksView_SecondSortProperty_ByDescent = _settings.SecondSortProperty_ByDescend;
        Settings.Default.BooksView_ThirdSortBookProperty = _settings.ThirdSortBookProperty;
        Settings.Default.BooksView_ThirdSortProperty_ByDescent = _settings.ThirdSortProperty_ByDescend;
        Settings.Default.Book_MaxContentLength = Settings.Default.Book_MaxContentLength;


        Settings.Default.Save();
    }

    #region Dictionaries   
    /// <summary>
    /// Gets an array of search fields available for the FindBooks page.
    /// </summary>
    public EBibliographicKindInformation[] SearchFields => _settings.SearchFields;

    /// <summary>
    /// Gets an array of boolean values representing the state of various settings.
    /// </summary>
    public bool[] Booleans => _settings.Booleans;

    /// <summary>
    /// Gets an array of boolean values representing the state of various settings.
    /// </summary>
    public string[] BookProperties => _settings.BookProperties;
    #endregion

    #region Book details
    /// <summary>
    /// Gets or sets the maximum content length for a book.
    /// </summary>
    public long Book_MaxContentLength
    {
        get => _settings.Book_MaxContentLength;
        set
        {
            var tooltip = string.Empty;
            if (value < 0)
            {
                _settings.Book_MaxContentLength = 0;
                Book_MaxContentLength_ToolTip = "0";
            }
            else if (SetProperty(ref _settings.Book_MaxContentLength, value))
            {
                Book_MaxContentLength_ToolTip = GetFileSizeTooltip(value);
            }
        }
    }

    /// <summary>
    /// Gets or sets the tooltip for the maximum content length for a book.
    /// </summary>
    public string Book_MaxContentLength_ToolTip
    {
        get => _book_MaxContentLength_ToolTip;
        set => SetProperty(ref _book_MaxContentLength_ToolTip, value);
    }
    #endregion

    #region BooksViewModel Page
    /// <summary>
    /// Gets or sets the primary property used for sorting books.
    /// </summary>
    public string FirstSortBookProperty
    {
        get => _settings.FirstSortBookProperty;
        set => SetProperty(ref _settings.FirstSortBookProperty, value);
    }
    public bool FirstSortProperty_ByDescend
    {
        get => _settings.FirstSortProperty_ByDescend;
        set => SetProperty(ref _settings.FirstSortProperty_ByDescend, value);
    }

    /// <summary>
    /// Gets or sets the secondary property used for sorting books.
    /// </summary>
    public string SecondSortBookProperty
    {
        get => _settings.SecondSortBookProperty;
        set => SetProperty(ref _settings.SecondSortBookProperty, value);
    }
    public bool SecondSortProperty_ByDescend
    {
        get => _settings.SecondSortProperty_ByDescend;
        set => SetProperty(ref _settings.SecondSortProperty_ByDescend, value);
    }

    /// <summary>
    /// Gets or sets the tertiary property used for sorting books.
    /// </summary>
    public string ThirdSortBookProperty
    {
        get => _settings.ThirdSortBookProperty;
        set => SetProperty(ref _settings.ThirdSortBookProperty, value);
    }
    public bool ThirdSortProperty_ByDescend
    {
        get => _settings.ThirdSortProperty_ByDescend;
        set => SetProperty(ref _settings.ThirdSortProperty_ByDescend, value);
    }

    public string SortBookByPropertiesTooltip
    {
        get => _settings.SortBookByPropertiesTooltip;
        set => SetProperty(ref _settings.SortBookByPropertiesTooltip, value);
    }
    #endregion

    #region FindBooks Page
    /// <summary>
    /// Gets or sets the currently selected search field for the FindBooks page.
    /// </summary>
    public EBibliographicKindInformation SearchField
    {
        get => _settings.SearchField;
        set => SetProperty(ref _settings.SearchField, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether to search on the fly for the FindBooks page.
    /// </summary>
    public bool SearchOnFly
    {
        get => _settings.SearchOnFly;
        set => SetProperty(ref _settings.SearchOnFly, value);
    }
    #endregion

    #region Debug Page
    /// <summary>
    /// Gets or sets the font size for the debug text.
    /// </summary>
    public double Debug_TextFontSize
    {
        get => _settings.Debug_TextFontSize;
        set
        {
            if (6 < value && value < 28)
                SetProperty(ref _settings.Debug_TextFontSize, value);
        }
    }
    #endregion


    #region private methods
    /// <summary>
    /// Attempts to parse a string setting into an enumeration value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="key">The string key to parse.</param>
    /// <param name="_value">The reference to the enumeration value to be updated.</param>
    /// <param name="Value">The enumeration value to raise a property changed event for.</param>
    /// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>
    private bool TryConvertStringToEnum<T>(string key, ref T _value, T Value) where T : Enum
    {
        var result = false;
        try
        {
            _value = (T)Enum.Parse(typeof(T), key);
            RaisePropertyChanged(nameof(Value));
            result = true;
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// Returns a tooltip text describing the file size.
    /// </summary>
    /// <param name="fileSize">The size of the file in bytes.</param>
    /// <returns>A tooltip text describing the file size.</returns>
    private string GetFileSizeTooltip(long fileSize)
    {
        var tooltip = string.Empty;

        if (1_000_000_000 < fileSize)
        {
            var fileSizeInGigabytes = fileSize / 1_000_000_000;
            tooltip = $"The set file size enormous ({fileSizeInGigabytes} GB). This may cause storage issues.";
        }
        else
        {
            // Convert the file size to a human-readable format
            ConvertToHumanReadableFileSize(fileSize, EFileLengthUnit.Byte, out var fileSizeInUnits, out var unit);

            tooltip = $"The loaded file size can be maiximum as {fileSizeInUnits} {unit}";
        }

        return tooltip;
    }

    /// <summary>
    /// Converts a file size in bytes to a human-readable format.
    /// </summary>
    /// <param name="number">The file size in bytes.</param>
    /// <param name="startLength">The starting unit of measurement (e.g. Byte, KB, MB, GB).</param>
    /// <param name="result">The converted file size.</param>
    /// <param name="length">The unit of measurement for the converted file size.</param>
    private void ConvertToHumanReadableFileSize(long number, EFileLengthUnit startLength, out long result, out EFileLengthUnit length)
    {
        result = number;
        length = startLength;
        if (1024 < number && (int)startLength < (int)EFileLengthUnit.GB)
        {
            length = startLength.Next();
            result = number / 1024;
            ConvertToHumanReadableFileSize(result, length, out result, out length);
        }
    }
    #endregion


    private readonly SettingsModel _settings;
    private string _book_MaxContentLength_ToolTip;
}


