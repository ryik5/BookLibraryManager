using BookLibraryManager.Common;

namespace LibraryManager.Models;

/// <author>YR 2025-02-14</author>
internal sealed class SettingsModel
{
    public SettingsModel()
    {
        SearchFields = Enum.GetValues(typeof(EBibliographicKindInformation)).Cast<EBibliographicKindInformation>().ToArray();
        Bools = [true,false];
    }

    #region FindBooks ViewModel
    public EBibliographicKindInformation[] SearchFields;
    public EBibliographicKindInformation SearchField = EBibliographicKindInformation.All;

    public bool[] Bools;
    public bool SearchOnFly = false;
    #endregion


    #region BookDetails ViewModel
    public bool ShowBookDetails = false;
    #endregion
}
