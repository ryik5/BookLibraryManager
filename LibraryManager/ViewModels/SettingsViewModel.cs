using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Properties;

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
        TryParseStringSettings(Settings.Default.FindBooks_LastSearchField, ref _settings.SearchField, SearchField);
        _settings.SearchOnFly = Settings.Default.FindBooks_SearchOnFly;
        _settings.Debug_TextFontSize = Settings.Default.Debug_TextFontSize;
    }

    /// <summary>
    /// Saves all application settings to the settings store.
    /// </summary>
    public void SaveSettings()
    {
        Settings.Default.FindBooks_LastSearchField = _settings.SearchField.ToString();
        Settings.Default.FindBooks_SearchOnFly = _settings.SearchOnFly;
        Settings.Default.Debug_TextFontSize = _settings.Debug_TextFontSize;

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
    public bool[] Bools => _settings.Bools;
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




    /// <summary>
    /// Attempts to parse a string setting into an enumeration value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the enumeration.</typeparam>
    /// <param name="key">The string key to parse.</param>
    /// <param name="_value">The reference to the enumeration value to be updated.</param>
    /// <param name="Value">The enumeration value to raise a property changed event for.</param>
    /// <returns><c>true</c> if the parsing was successful; otherwise, <c>false</c>.</returns>
    private bool TryParseStringSettings<T>(string key, ref T _value, T Value) where T : Enum
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


    private SettingsModel _settings;
}
