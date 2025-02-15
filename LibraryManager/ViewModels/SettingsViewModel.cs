﻿using BookLibraryManager.Common;
using LibraryManager.Models;
using LibraryManager.Properties;

namespace LibraryManager.ViewModels;

/// <author>YR 2025-02-14</author>
internal sealed class SettingsViewModel : BindableBase
{
    public SettingsViewModel(SettingsModel settings)
    {
        _settings = settings;
        LoadAllSettings();
    }

    public void ResetAllSettings()
    {
        Settings.Default.Properties.Cast<System.ComponentModel.PropertyDescriptor>().ToList().ForEach(p => p.ResetValue(Settings.Default));
    }

    public void LoadAllSettings()
    {
        TryParseStringSettings(Settings.Default.FindBooks_LastSearchField, ref _settings.SearchField, SearchField);
        _settings.SearchOnFly = Settings.Default.FindBooks_SearchOnFly;
    }

    public void SaveSettings()
    {
        Settings.Default.FindBooks_LastSearchField = _settings.SearchField.ToString();
        Settings.Default.FindBooks_SearchOnFly = _settings.SearchOnFly;
        Settings.Default.Save();
    }

    public EBibliographicKindInformation[] SearchFields => _settings.SearchFields;
    public EBibliographicKindInformation SearchField
    {
        get => _settings.SearchField;
        set => SetProperty(ref _settings.SearchField, value);
    }

    public bool[] Bools => _settings.Bools;
    public bool SearchOnFly
    {
        get => _settings.SearchOnFly;
        set => SetProperty(ref _settings.SearchOnFly, value);
    }


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
