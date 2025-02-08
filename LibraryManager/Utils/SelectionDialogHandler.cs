using System.Windows.Media.Imaging;
using BookLibraryManager.Common;
using Microsoft.Win32;

namespace LibraryManager.Utils;

/// <summary>
/// Handles the selection of media data through a dialog.
/// </summary>
/// <author>YR 2025-01-27</author>
internal class SelectionDialogHandler
{
    /// <summary>
    /// Opens a dialog for selecting a media file and returns the selected media data.
    /// </summary>
    /// <returns>The selected media data, or null if no file was selected.</returns>
    public MediaData? SelectMediaData()
    {
        var op = new OpenFileDialog();
        op.Title = "Select a picture";
        op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
          "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
          "Portable Network Graphic (*.png)|*.png";
        if (op.ShowDialog() == true)
        {
            var img = new MediaData();
            img.Name = $"{nameof(BitmapImage)}";
            img.OriginalPath = op.FileName;
            img.Image = new BitmapimageConvertor().BitmapImage2Bitmap(new BitmapImage(new Uri(op.FileName)));

            return img;
        }

        return null;
    }


    /// <summary>
    /// Returns the path to the XML file with the library.
    /// </summary>
    public string? GetPathToXmlFile()
    {
        var openDialog = new OpenFileDialog()
        {
            Title = "Load the library",
            DefaultExt = ".xml",
            Filter = "XML Library (.xml)|*.xml"
        };
        var dialogResult = openDialog.ShowDialog();
        if (!dialogResult.HasValue || !dialogResult.Value)
            return null;

        var path = openDialog.FileName;

        return path;
    }

    /// <summary>
    /// Returns the path to the folder to store the library.
    /// </summary>
    public string? GetPathToFolder(string windowTitle)
    {
        var openDialog = new OpenFolderDialog()
        {
            Title = windowTitle,
            Multiselect = false,
            ValidateNames = true
        };
        var dialogResult = openDialog.ShowDialog();
        if (!dialogResult.HasValue || !dialogResult.Value)
            return null;

        return openDialog.FolderName;
    }
}
