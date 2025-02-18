using BookLibraryManager.Common;

namespace LibraryManager.Models;

/// <summary>
/// Represents a model for storing application settings.
/// </summary>
/// <author>YR 2025-02-14</author>
internal sealed class SettingsModel
{
    public SettingsModel()
    {
        SearchFields = Enum.GetValues(typeof(EBibliographicKindInformation)).Cast<EBibliographicKindInformation>().ToArray();
        Bools = [true, false];
    }

    #region Dictionaries   
    /// <summary>
    /// Gets or sets an array of search fields available for the FindBooks page.
    /// </summary>
    public EBibliographicKindInformation[] SearchFields;

    /// <summary>
    /// Gets or sets an array of boolean values representing the state of various settings.
    /// </summary>
    public bool[] Bools;
    #endregion


    #region FindBooks Page
    /// <summary>
    /// Gets or sets the currently selected search field for the FindBooks page.
    public EBibliographicKindInformation SearchField = EBibliographicKindInformation.All;
    /// <summary>
    /// Gets or sets a value indicating whether to search on the fly for the FindBooks page.
    /// </summary>
    public bool SearchOnFly = false;
    #endregion


    #region BookDetails Page
    /// <summary>
    /// Gets or sets a value indicating whether to show book details.
    /// </summary>
    public bool ShowBookDetails = false;
    #endregion

    #region Debug Page
    /// <summary>
    /// Gets or sets the font size for the debug text.
    /// </summary>
    public double Debug_TextFontSize = 12;
    #endregion
}
